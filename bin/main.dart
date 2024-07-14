import 'dart:io';
import "parser/protobuf_parser.dart";
import 'generater/unreal_object_generator.dart';

const version = "1.0.0";

void main() {
  final protoFilePath = 'FTGTestingProtobuf.proto';
  var protoContent = File(protoFilePath).readAsStringSync();
  final parser = UnrealProtobufParser(protoContent);
  parser.parseMessages();
  parser.parseEnumMessages();
  print(parser.getMessages);
  final objectGenerator = UnrealObjectGenerator(protoFilePath, version);
  for (var message in parser.getMessages) {
    objectGenerator.generateMessageCode(message);
  }
  print(objectGenerator.getGeneratedUnrealCode);
}
