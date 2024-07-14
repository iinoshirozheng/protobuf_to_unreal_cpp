class ProtobufRegExp {
  // Remove single-line comments (// comment)
  static final singleLineComments = RegExp(r'//.*');
  // Remove multi-line comments (/* comment */)
  static final multiLineComments = RegExp(r'/\*.*?\*/', dotAll: true);
  static final message = RegExp(r'message\s+(\w+)\s*\{([^}]+)\}');
  static final messageField =
      RegExp(r'(repeated\s+\w+|map<\w+,\s*\w+>|\w+)\s+(\w+)\s*=\s*(\d+);');
  static final arrayTypeField = RegExp(r'(repeated\s+\w+)');
  static final mapTypeField = RegExp(r'map<(\w+),\s*(\w+)>');
  static final mapTypeSplitField = RegExp(r'(\w+)<(\w+),\s*(\w+)');
  static final enumMessage = RegExp(r'enum\s+(\w+)\s*\{([^}]+)\}');
  static final enumMessageField = RegExp(r'(\w+)\s*=\s*(\d+);');
}

const String defaultTypeMatchingProtobufToUE = 'protobuf_2_ue';
const String defaultTypeMatchingYaml = '''
# protobuf to unreal c++ types 
$defaultTypeMatchingProtobufToUE:
  string: FString
  int32: int32
  int64: int64
  uint32: uint32
  uint64: uint64
  sint32: int32
  sint64: int64
  fixed32: uint32
  fixed64: uint64
  sfixed32: int32
  sfixed64: int64
  float: float
  double: double
  bool: bool
  bytes: TArray<uint8>
  repeated: TArray
  map: TMap
''';
