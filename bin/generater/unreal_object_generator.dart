import "../generater/generater_option.dart";
import '../prototype/protobuf_message.dart';
import '../matching/type_matching.dart';

class UnrealObjectGenerator {
  late String protoFilePath;
  late StringBuffer buffer;
  final matchType = TypeMatching();
  static const String tab = "    ";
  get getGeneratedUnrealCode => buffer.toString();
  late GenerateOption genOption;
  UnrealObjectGenerator(this.protoFilePath, currentVersion) {
    generateHeaderCode();
    genOption = GenerateOption(currentVersion);
  }

  void generateHeaderCode() {
    buffer = StringBuffer()
      ..writeln('#pragma once')
      ..writeln()
      ..writeln('#include "CoreMinimal.h"')
      ..writeln('#include "${protoFilePath.split(".").first}.generated.h"');
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
    buffer.writeln('public:');
    buffer.writeln('${tab}FORCEINLINE $className() {}');
    buffer.writeln();
    buffer.writeln('};');
  }

  void generateClassCode(ProtobufMessage message) {
    var className = '${genOption.parentClass.name}_${message.name}';
    buffer.writeln();
    buffer.writeln(genOption.childClass.classMacro);
    buffer.write('class ${genOption.projectName}_API ');
    buffer.write('$className ');
    buffer.writeln(': public ${genOption.parentClass.name}');
    buffer.writeln('{');
    buffer.writeln('${tab}GENERATED_BODY()');
    buffer.writeln('public:');
    buffer.writeln('${tab}FORCEINLINE $className() {}');
    buffer.writeln();
    for (final field in message.fields) {
      buffer.writeln('$tab${genOption.childClass.propertyMacro}');
      buffer.writeln(
          '$tab${matchType.mapProtobufToUnrealType(field.type)} ${field.name};');
      buffer.writeln();
    }
    buffer.writeln('};');
  }

  void generateEnumCode(ProtobufMessage protobufEnum) {
    var className = 'E${protobufEnum.name}';
    buffer.writeln();
    buffer.writeln(genOption.enumClassMacro);
    buffer.writeln('enum class $className : uint8');
    buffer.writeln('{');
    for (final field in protobufEnum.fields) {
      buffer.writeln('$tab${field.name} = ${field.number},');
    }
    buffer.writeln('${tab}Size');
    buffer.writeln('};');
  }
}
