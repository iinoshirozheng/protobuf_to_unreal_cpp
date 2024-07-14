import '../matching/matching_define.dart';
import '../prototype/protobuf_message.dart';

class UnrealProtobufParser {
  late final String content;
  List<ProtobufMessage> protobufMessages = [];
  List<ProtobufMessage> protobufEnums = [];
  get getMessages => protobufMessages;
  get getEnums => protobufEnums;
  UnrealProtobufParser(String protobufContent) {
    content = removeComments(protobufContent);
  }

  String removeComments(String content) {
    content = content.replaceAll(ProtobufRegExp.singleLineComments, '');
    content = content.replaceAll(ProtobufRegExp.multiLineComments, '');
    return content;
  }

  String parseMessageBody(RegExpMatch messageMatched) {
    final start = messageMatched.end;
    int braceCount = 1;
    for (int findEnd = start; findEnd < content.length; findEnd++) {
      if (content[findEnd] == '{') braceCount++;
      if (content[findEnd] == '}') braceCount--;
      if (braceCount == 0) {
        return content.substring(start, findEnd);
      }
    }
    return ""; // Error case
  }

  void parseMessages() {
    for (final messageMatch in ProtobufRegExp.message.allMatches(content)) {
      final messageName = messageMatch.group(1) ?? "";
      final messageBody = parseMessageBody(messageMatch);
      final fields = <ProtobufMessageField>[];
      for (final fieldMatch
          in ProtobufRegExp.messageField.allMatches(messageBody)) {
        final fieldType = fieldMatch.group(1) ?? "";
        final fieldName = fieldMatch.group(2) ?? "";
        final fieldNumber = fieldMatch.group(3) ?? "";

        fields.add(ProtobufMessageField(fieldType, fieldName, fieldNumber));
      }

      protobufMessages.add(ProtobufMessage(messageName, fields));
    }
  }

  void parseEnums() {
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

      protobufEnums.add(ProtobufMessage(messageName, fields));
    }
  }
}
