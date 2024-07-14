import 'dart:io';
import "parser/protobuf_parser.dart";
import 'generater/unreal_object_generator.dart';

const version = "1.0.0";

void main() {
  final protoFilePath = 'FTGTestingProtobuf.proto';
  var protoContent = File(protoFilePath).readAsStringSync();
  final parser = UnrealProtobufParser(protoContent)
    ..parseMessages()
    ..parseEnums();

  final objectGenerator = UnrealObjectGenerator(protoFilePath, version);
  objectGenerator.generateBaseClassCode();
  for (var message in parser.getMessages) {
    objectGenerator.generateClassCode(message);
  }
  for (var enumMessage in parser.protobufEnums) {
    objectGenerator.generateEnumCode(enumMessage);
  }
  print(objectGenerator.getGeneratedUnrealCode);
}
