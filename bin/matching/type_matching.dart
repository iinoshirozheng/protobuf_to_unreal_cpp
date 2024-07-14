import 'dart:io';
import 'package:yaml/yaml.dart';
import 'matching_define.dart';

class TypeMatching {
  late Map<String, String> proto2UEMap;

  TypeMatching() {
    // Load the configuration file.
    final File matcingFile = File('type_matching.yaml');
    if (!matcingFile.existsSync()) {
      print('找不到設定檔案，自動生成 type_matching.yaml 檔案 （在執行檔同層資料夾');
      matcingFile.writeAsStringSync(defaultTypeMatchingYaml);
    }
    final yamlMap = loadYaml(matcingFile.readAsStringSync()) as YamlMap;
    proto2UEMap =
        Map<String, String>.from(yamlMap[defaultTypeMatchingProtobufToUE]);
  }

  String mapProtobufToUnrealType(String protoTypeString) {
    // Check if protoType exists in the proto2UEMap
    if (proto2UEMap.containsKey(protoTypeString)) {
      return proto2UEMap[protoTypeString]!;
    }

    if (isArray(protoTypeString)) {
      var typeArr = protoTypeString.split(' ');
      return '${mapProtobufToUnrealType(typeArr.first)}<${mapProtobufToUnrealType(typeArr.last)}>';
    }

    if (isMap(protoTypeString)) {
      var mapType =
          ProtobufRegExp.mapTypeSplitField.firstMatch(protoTypeString);
      if (mapType != null && mapType.groupCount == 3) {
        String mapStr = mapProtobufToUnrealType(mapType.group(1)!);
        String mapKey = mapProtobufToUnrealType(mapType.group(2)!);
        String mapValue = mapProtobufToUnrealType(mapType.group(3)!);
        return '$mapStr<$mapKey, $mapValue>';
      }
    }

    throw UnimplementedError('Type "$protoTypeString" is not implemented.');
  }

  bool isArray(String protoTypeString) =>
      protoTypeString.contains(ProtobufRegExp.arrayTypeField);

  bool isMap(String protoTypeString) =>
      protoTypeString.contains(ProtobufRegExp.mapTypeField);
}
