import "generator_option.dart";
import '../prototype/protobuf_message.dart';
import '../matching/type_matching.dart';
import '../matching/matching_define.dart';
import '../parser/protobuf_parser.dart';

class UnrealObjectGenerator {
  late String protoFilePath;
  late StringBuffer buffer;
  late StringBuffer bufferCpp;
  final matchType = TypeMatching();
  final UnrealProtobufParser? parser;
  static const String tab = "    ";
  get getGeneratedUnrealHeaderCode => buffer.toString();
  get getGeneratedUnrealSourceCode => bufferCpp.toString();
  get getPrivateProtobufName => '_ProtobufMessage';
  get getMessageName => 'Message';
  get getNameSpace => parser?.namespace ?? "";
  get getProtoFileName =>
      ProtobufRegExp.protoFileName.firstMatch(protoFilePath)?.group(1) ?? "";
  late GenerateOption genOption;

  void generateEnumsCode() => parser?.protobufEnums.forEach(generateEnumCode);
  void generateClassesCode() =>
      parser?.protobufMessages.forEach(generateClassCode);

  UnrealObjectGenerator(this.protoFilePath, this.parser, currentVersion) {
    generateHeaderCode();
    genOption = GenerateOption(currentVersion);
  }

  void recordProtoName() {
    if (parser == null) {
      return;
    }
    for (var message in parser!.protobufMessages) {
      TypeMatching.recordProtobufTransferName(
          message.name, '${genOption.parentClass.name}_${message.name}');
    }

    for (var enumMessage in parser!.protobufEnums) {
      TypeMatching.recordProtobufTransferName(
          enumMessage.name, 'EProto_${enumMessage.name}');
    }
  }

  void generateHeaderCode() {
    buffer = StringBuffer()
      ..writeln('#pragma once')
      ..writeln()
      ..writeln('#include "CoreMinimal.h"')
      ..writeln('#include "UObject/NoExportTypes.h"')
      // ..writeln('#include "${protoFilePath.split(".").first}.pb.h"')
      ..writeln('#include "Protobuf_$getProtoFileName.generated.h"');
    bufferCpp = StringBuffer();
  }

  void generateBaseClassCode() {
    var className = genOption.parentClass.name;
    buffer.writeln();
    buffer.writeln(genOption.childClass.classMacro);
    buffer.write('class ${genOption.projectName}_API ');
    buffer.write('$className ');
    buffer.writeln(': public ${genOption.parentClass.parentClass}');
    buffer.writeln('{');
    buffer.writeln('${tab}GENERATED_BODY()');
    buffer.writeln('};');
  }

  void generateEnumCode(ProtobufMessage protobufEnum) {
    buffer.writeln();
    buffer.writeln(genOption.enumClassMacro);
    buffer.writeln('enum class EProto_${protobufEnum.name} : uint8');
    buffer.writeln('{');
    for (final field in protobufEnum.fields) {
      buffer.writeln('$tab${field.name} = ${field.number},');
    }
    buffer.writeln('${tab}Size');
    buffer.writeln('};');
  }

  void generateClassCode(ProtobufMessage message) {
    var className = TypeMatching.getProtobufTransferName(message.name);
    buffer.writeln();
    buffer.writeln(genOption.childClass.classMacro);
    buffer.write('class ${genOption.projectName}_API ');
    buffer.write('UProto_${message.name} ');
    buffer.writeln(': public ${genOption.parentClass.parentClass}');
    buffer.writeln('{');
    buffer.writeln('${tab}GENERATED_BODY()');
    buffer.writeln('public:');

    // 生成構造函數實現
    bool hasSubobjects =
        message.fields.any((field) => matchType.isProtobuf(field.protoType));

    // 添加構造函數
    if (hasSubobjects) {
      buffer.writeln('$tab$className();');
      buffer.writeln();
    }
    for (final field in message.fields) {
      if (matchType.isSupportUProperty(field.protoType)) {
        buffer.writeln('$tab${genOption.childClass.propertyMacro}');
      } else {
        print(
            'Warning : Type "${field.protoType}" was not support Unreal Blueprint type , CANT USED MACRO "UPROPERTY"');
      }
      buffer.writeln(
          '$tab${matchType.toUnrealType(field.protoType)} ${field.name} ${matchType.isProtobuf(field.protoType) ? '= nullptr' : ''};');
      buffer.writeln();
    }

    buffer.writeln('};');

    // 生成構造函數實現
    if (hasSubobjects) {
      bufferCpp.writeln();
      bufferCpp.writeln('$className::$className()');
      bufferCpp.writeln('{');
      for (final field in message.fields) {
        if (matchType.isProtobuf(field.protoType)) {
          // 修改這裡，移除額外的指針
          bufferCpp.writeln(
              '$tab${field.name} = CreateDefaultSubobject<${matchType.toUnrealType(field.protoType).replaceAll('*', '')}>(TEXT("${field.name}"));');
        }
      }
      bufferCpp.writeln('}');
    }
  }

  void generateFunctionToProtobuf(ProtobufMessage message) {
    buffer.writeln(
        '${tab}FORCEINLINE const $getNameSpace${message.name}& ToProtobuf()');
    buffer.writeln('$tab{');
    message.fields.forEach(_processFieldToProtobuf);
    buffer.writeln('$tab${tab}return $getPrivateProtobufName;');
    buffer.writeln('$tab};');
    buffer.writeln();
  }

  void generateFunctionSetProtobuf(ProtobufMessage message) {
    buffer.writeln(
        '${tab}FORCEINLINE void SetProtobuf(const $getNameSpace${message.name}& $getMessageName)');
    buffer.writeln('$tab{');
    for (var field in message.fields) {
      _processFieldSetProtobuf(field);
    }
    buffer.writeln('$tab$tab$getPrivateProtobufName.CopyFrom($getMessageName)');
    buffer.writeln('$tab};');
    buffer.writeln();
  }

  void _processFieldSetProtobuf(ProtobufMessageField field) {
    switch (field.protoType) {
      case var type when matchType.isProtobuf(type):
        buffer.writeln(
            '$tab$tab${field.name}->SetProtobuf($getMessageName.${field.name.toLowerCase()}());');
        break;

      case var type when matchType.isEnum(type):
        buffer.writeln(
            '$tab$tab${field.name} = (EProto_${field.protoType})$getMessageName.${field.name.toLowerCase()}();');
        break;

      case var type when matchType.isArray(type):
        _processSetArray(field);
        break;

      case var type when matchType.isMap(type):
        _processSetMap(field);
        break;

      default:
        buffer
            .writeln('$tab$tab${field.name} = ${processValueConvert(field)};');
        break;
    }
  }

  void _processFieldToProtobuf(ProtobufMessageField field) {
    switch (field.protoType) {
      case var type when matchType.isProtobuf(type):
        buffer.write('$tab$tab$getPrivateProtobufName');
        buffer.write('.mutable_${field.name.toLowerCase()}()->');
        buffer.writeln('MergeFrom(${field.name}->ToProtobuf());');
        break;

      case var type when matchType.isEnum(type):
        buffer.write('$tab$tab$getPrivateProtobufName');
        buffer.write('.set_${field.name.toLowerCase()}');
        buffer.writeln('(($getNameSpace$type)${field.name});');
        break;

      case var type when matchType.isArray(type):
        _processArray(field);
        break;

      case var type when matchType.isMap(type):
        _processMap(field);
        break;

      default:
        buffer.write('$tab$tab$getPrivateProtobufName');
        buffer.write('.set_${field.name.toLowerCase()}');
        buffer.writeln('(${processValueConvert(field)});');
        break;
    }
  }

  void _processArray(ProtobufMessageField field) {
    buffer.write('$tab$tab$getPrivateProtobufName');
    buffer
        .write('.clear_${field.name.toLowerCase()}();'); // Clear existing array
    buffer.writeln();

    buffer
        .writeln('$tab${tab}for (const auto& item : ${field.name}->ToArray())');
    buffer.writeln('$tab$tab{');
    buffer.write('$tab$tab$tab');

    // Assuming array elements are primitive or Protobuf messages
    if (matchType.isProtobuf(field.protoType)) {
      buffer.write('$getPrivateProtobufName');
      buffer.write('.add_${field.name.toLowerCase()}()->');
      buffer.writeln('MergeFrom(item.ToProtobuf());');
    } else if (matchType.isEnum(field.protoType)) {
      buffer.write('$getPrivateProtobufName');
      buffer.write('.add_${field.name.toLowerCase()}(item);');
    } else {
      buffer.write('$getPrivateProtobufName');
      buffer.write('.add_${field.name.toLowerCase()}(item);');
    }
    buffer.writeln();
    buffer.writeln('$tab$tab}');
  }

  void _processMap(ProtobufMessageField field) {
    buffer.write('$tab$tab$getPrivateProtobufName');
    buffer.write('.clear_${field.name.toLowerCase()}();'); // Clear existing map
    buffer.writeln();

    buffer.write('$tab$tab{');
    buffer.writeln();

    buffer.write('$tab$tab$tab'); // Indent for the loop

    buffer.writeln('for (const auto& [key, value] : ${field.name}->ToMap()) {');
    buffer.write('$tab$tab$tab$tab');

    // Assuming map keys and values are primitive or Protobuf messages
    if (matchType.isProtobuf(field.protoType)) {
      buffer.write('$getPrivateProtobufName');
      buffer.write('.mutable_${field.name.toLowerCase()}()->');
      buffer.writeln('insert({key, value.ToProtobuf()});');
    } else if (matchType.isEnum(field.protoType)) {
      buffer.write('$getPrivateProtobufName');
      buffer.write('.mutable_${field.name.toLowerCase()}()->');
      buffer.writeln('insert({key, value});');
    } else {
      buffer.write('$getPrivateProtobufName');
      buffer.write('.mutable_${field.name.toLowerCase()}()->');
      buffer.writeln('insert({key, value});');
    }

    buffer.writeln('$tab$tab}');
    buffer.writeln('$tab$tab}');
  }

  void _processSetArray(ProtobufMessageField field) {
    buffer.writeln('$tab$tab${field.name}.Empty();');
    buffer.writeln(
        '$tab${tab}for (int32 i = 0; i < $getMessageName.${field.name.toLowerCase()}_size(); i++)');
    buffer.writeln('$tab$tab{');

    if (matchType.isProtobuf(field.protoType)) {
      buffer.writeln(
          '$tab$tab$tab${field.name}.Add(NewObject<UProtobuf_${field.protoType}>());');
      buffer.writeln(
          '$tab$tab$tab${field.name}.Last()->SetProtobuf($getMessageName.${field.name.toLowerCase()}(i));');
    } else if (matchType.isEnum(field.protoType)) {
      buffer.writeln(
          '$tab$tab$tab${field.name}.Add((EProto_${field.protoType})$getMessageName.${field.name.toLowerCase()}(i));');
    } else {
      buffer.writeln(
          '$tab$tab$tab${field.name}.Add($getMessageName.${field.name.toLowerCase()}(i));');
    }

    buffer.writeln('$tab$tab}');
  }

  void _processSetMap(ProtobufMessageField field) {
    buffer.writeln('${field.name}.Empty();');
    buffer.writeln(
        'for (const auto& [key, value] : $getMessageName.${field.name.toLowerCase()}())');
    buffer.writeln('$tab{');

    if (matchType.isProtobuf(field.protoType)) {
      buffer.writeln(
          '$tab${field.name}.Add(key, NewObject<UProtobuf_${field.protoType}>());');
      buffer.writeln('$tab${field.name}[key]->SetProtobuf(value);');
    } else if (matchType.isEnum(field.protoType)) {
      buffer.writeln(
          '$tab${field.name}.Add(key, (EProto_${field.protoType})value);');
    } else {
      buffer.writeln('$tab${field.name}.Add(key, value);');
    }

    buffer.writeln('$tab}');
  }

  String processValueConvert(ProtobufMessageField field) {
    switch (field.protoType) {
      case "string":
        return "TCHAR_TO_UTF8(*${field.name})";
      case "FString":
        return "UTF8_TO_TCHAR($getMessageName.${field.name.toLowerCase()}().c_str())";
      default:
        return field.name;
    }
  }
}
