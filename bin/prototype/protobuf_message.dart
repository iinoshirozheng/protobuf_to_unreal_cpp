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
