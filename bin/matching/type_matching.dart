import 'dart:io';
import 'package:yaml/yaml.dart';
import 'matching_define.dart';

class TypeMatching {
  late Map<String, String> proto2UEMap;
  static Map<String, String> protoFileMessageTransferMap = {};

  TypeMatching() {
    // Load the configuration file.
    final File matchingFile = File(
        '${Directory(File(Platform.resolvedExecutable).parent.path).path}/$defaultFilePathTypeMatching');
    if (!matchingFile.existsSync()) {
      print(
          'Configuration file not found. Automatically generating type_matching.yaml file (in the same directory as the executable)');
      matchingFile.writeAsStringSync(defaultTypeMatchingYaml);
    }
    final yamlMap = loadYaml(matchingFile.readAsStringSync()) as YamlMap;
    proto2UEMap =
        Map<String, String>.from(yamlMap[defaultTypeMatchingProtobufToUE]);
  }

  static void recordProtobufTransferName(
      String protoMessage, String ueMessage) {
    protoFileMessageTransferMap[protoMessage] = ueMessage;
  }

  static String getProtobufTransferName(String protoMessage) {
    return protoFileMessageTransferMap[protoMessage]!;
  }

  String toUnrealType(String protoTypeString) =>
      _mapProtobufToUnrealType(protoTypeString);

  String _mapProtobufToUnrealType(String protoTypeString) {
    // Check if protoType exists in the proto2UEMap
    if (proto2UEMap.containsKey(protoTypeString)) {
      return proto2UEMap[protoTypeString]!;
    }

    if (isArray(protoTypeString)) {
      var typeArr = protoTypeString.split(' ');
      return '${_mapProtobufToUnrealType(typeArr.first)}<${_mapProtobufToUnrealType(typeArr.last)}>';
    }

    if (isMap(protoTypeString)) {
      var mapType =
          ProtobufRegExp.mapTypeSplitField.firstMatch(protoTypeString);
      if (mapType != null && mapType.groupCount == 3) {
        String mapStr = _mapProtobufToUnrealType(mapType.group(1)!);
        String mapKey = _mapProtobufToUnrealType(mapType.group(2)!);
        String mapValue = _mapProtobufToUnrealType(mapType.group(3)!);
        return '$mapStr<$mapKey, $mapValue>';
      }
    }

    if (isProtobuf(protoTypeString)) {
      // 檢查型態是否為自己寫的 Message
      return '${protoFileMessageTransferMap[protoTypeString]}*';
    }

    if (isEnum(protoTypeString)) {
      return protoFileMessageTransferMap[protoTypeString]!;
    }

    throw UnimplementedError('Type: "$protoTypeString" is not supported!');
  }

  bool isSupportUProperty(String protoTypeString) {
    if (isArray(protoTypeString)) {
      var unrealTypeString = toUnrealType(protoTypeString);
      if (unrealTypeString.contains('FString') ||
          (unrealTypeString.contains('int32') &&
              !unrealTypeString.contains('uint32')) ||
          unrealTypeString.contains('UProto')) return true;
    }

    if (isProtobuf(protoTypeString) ||
        isEnum(protoTypeString) ||
        protoTypeString.contains('string')) return true;

    if (protoTypeString.contains('int')) {
      return protoTypeString.contains('uint8') ||
              (protoTypeString.contains('int32') &&
                  !protoTypeString.contains('uint32'))
          ? true
          : false;
    }
    return false;
  }

  bool isProtobuf(String protoTypeString) =>
      protoFileMessageTransferMap[protoTypeString]?.startsWith('UProto_') ??
      false;

  bool isEnum(String protoTypeString) =>
      protoFileMessageTransferMap[protoTypeString]?.startsWith('EProto_') ??
      false;

  bool isArray(String protoTypeString) =>
      protoTypeString.contains(ProtobufRegExp.arrayTypeField);

  bool isMap(String protoTypeString) =>
      protoTypeString.contains(ProtobufRegExp.mapTypeField);
}
