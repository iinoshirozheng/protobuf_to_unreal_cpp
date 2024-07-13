class ProtobufMessageField {
  final String type;
  final String name;
  final String number;

  ProtobufMessageField(this.type, this.name, this.number);

  @override
  String toString() => 'Field(type: $type, name: $name, number: $number)';
}

class ProtobufMessage {
  final String name;
  final List<ProtobufMessageField> fields;

  ProtobufMessage(this.name, this.fields);

  @override
  String toString() => 'Message(name: $name, fields: $fields)';
}

class UnrealProtobufParser {
  final String content;

  UnrealProtobufParser(this.content);

  List<ProtobufMessage> parse() {
    final messages = <ProtobufMessage>[];

    final messageRegExp = RegExp(r'message\s+(\w+)\s*\{([^}]+)\}');
    final fieldRegExp = RegExp(r'(\w+)\s+(\w+)\s*=\s*(\d+);');

    for (final messageMatch in messageRegExp.allMatches(content)) {
      final messageName = messageMatch.group(1) ?? "";
      final messageBody = messageMatch.group(2) ?? "";

      final fields = <ProtobufMessageField>[];
      for (final fieldMatch in fieldRegExp.allMatches(messageBody)) {
        final fieldType = fieldMatch.group(1) ?? "";
        final fieldName = fieldMatch.group(2) ?? "";
        final fieldNumber = fieldMatch.group(3) ?? "";

        fields.add(ProtobufMessageField(fieldType, fieldName, fieldNumber));
      }

      messages.add(ProtobufMessage(messageName, fields));
    }

    return messages;
  }
}
