import "../parser/protobuf_parser.dart";
import "../generater/generater_option.dart";

class UnrealObjectGenerator {
  late String protoFilePath;
  late StringBuffer buffer;
  static const String tab = "    ";
  get getGeneratedUnrealCode => buffer.toString();
  GenerateOption genOption = GenerateOption();
  UnrealObjectGenerator(this.protoFilePath) {
    generateHeaderCode();
  }

  void generateHeaderCode() {
    buffer = StringBuffer()
      ..writeln('#pragma once')
      ..writeln()
      ..writeln('#include "CoreMinimal.h"')
      ..writeln('#include "${protoFilePath.split(".").first}.generated.h"');
  }

  void generateMessageCode(List<ProtobufMessage> messages) {
    for (final message in messages) {
      buffer.writeln();
      buffer.writeln(genOption.childClass.classMacro);
      buffer.write('class ${genOption.projectName}_API ');
      buffer.write('${genOption.parentClass.name}_${message.name} ');
      buffer.writeln(': public ${genOption.parentClass.name}');
      buffer.writeln('{');
      buffer.writeln('${tab}GENERATED_BODY()');
      buffer.writeln('$tab${genOption.parentClass.name}${message.name}() {}');
      buffer.writeln();
      buffer.writeln('public:');
      for (final field in message.fields) {
        buffer.writeln('$tab${genOption.childClass.propertyMacro}');
        buffer.writeln(
            '$tab${mapProtobufToUnrealType(field.type)} ${field.name};');
        buffer.writeln();
      }
      buffer.writeln('};');
    }
  }
}

String mapProtobufToUnrealType(String protoType) {
  switch (protoType) {
    case 'string':
      return 'FString';
    case 'int32':
      return 'int32';
    // Add more type mappings as needed
    default:
      throw UnimplementedError('Type $protoType is not implemented.');
  }
}
