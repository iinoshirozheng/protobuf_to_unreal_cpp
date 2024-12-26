class ProtobufMessageField {
  final String protoType;
  final String name;
  final String number;

  ProtobufMessageField(this.protoType, this.name, this.number);

  @override
  String toString() =>
      'Field(protoType: $protoType, name: $name, number: $number)';
}

class ProtobufMessage {
  final String name;
  final List<ProtobufMessageField> fields;

  ProtobufMessage(this.name, this.fields);

  @override
  String toString() => 'Message(name: $name, fields: $fields)';
}
