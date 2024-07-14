import '../matching/matching_define.dart';
import '../prototype/protobuf_message.dart';

class UnrealProtobufParser {
  final String content;
  List<ProtobufMessage> protobufMessage = [];
  get getMessages => protobufMessage;
  UnrealProtobufParser(this.content);

  void parseMessages() {
    for (final messageMatch in ProtobufRegExp.message.allMatches(content)) {
      final messageName = messageMatch.group(1) ?? "";
      final messageBody = messageMatch.group(2) ?? "";

      final fields = <ProtobufMessageField>[];
      for (final fieldMatch
          in ProtobufRegExp.messageField.allMatches(messageBody)) {
        final fieldType = fieldMatch.group(1) ?? "";
        final fieldName = fieldMatch.group(2) ?? "";
        final fieldNumber = fieldMatch.group(3) ?? "";

        fields.add(ProtobufMessageField(fieldType, fieldName, fieldNumber));
      }

      protobufMessage.add(ProtobufMessage(messageName, fields));
    }
  }

  void parseEnumMessages() {
    for (final messageMatch in ProtobufRegExp.enumMessage.allMatches(content)) {
      final messageName = messageMatch.group(1) ?? "";
      final messageBody = messageMatch.group(2) ?? "";

      final fields = <ProtobufMessageField>[];
      for (final fieldMatch
          in ProtobufRegExp.enumMessageField.allMatches(messageBody)) {
        final fieldName = fieldMatch.group(1) ?? "";
        final fieldValue = fieldMatch.group(2) ?? "";

        fields.add(ProtobufMessageField("enum", fieldName, fieldValue));
      }

      protobufMessage.add(ProtobufMessage(messageName, fields));
    }
  }
}
