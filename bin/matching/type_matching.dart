import 'dart:io';
import 'package:yaml/yaml.dart';
import 'matching_define.dart';

class TypeMatching {
  static Map<String, String> proto2UEMap = {};

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

  static String mapProtobufToUnrealType(String protoType) {
    if (proto2UEMap.containsKey(protoType)) {
      return proto2UEMap[protoType]!;
    } else {
      throw UnimplementedError('Type "$protoType" is not implemented.');
    }
  }
}
