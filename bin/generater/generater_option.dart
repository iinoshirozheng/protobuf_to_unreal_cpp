import 'package:yaml/yaml.dart';
import 'generater_define.dart';
import 'dart:io';

class UnrealClass {
  final String name;
  final String classMacro;
  final String propertyMacro;
  final String parentClass;

  UnrealClass(this.name, this.classMacro, this.propertyMacro, this.parentClass);
}

class GenerateOption {
  Exception throwException(String configName) => throw Exception(
      '$configName 設定錯誤，請檢查 config.yaml 確認參數格式如以下犯例：\n$defaultYamlFile');

  late String generatedCodeOutputDir;
  late String compilerVersion;
  late String projectName;
  late UnrealClass parentClass;
  late UnrealClass childClass;
  GenerateOption() {
    // Load the configuration file.
    final File configFile = File(configYamlPath);
    if (!configFile.existsSync()) {
      print('找不到設定檔案，自動生成 config.yaml 檔案 （在執行檔同層資料夾');
      configFile.writeAsStringSync(defaultYamlFile);
    }
    final String configString = configFile.readAsStringSync();
    final YamlMap config = loadYaml(configString);
    print('Configuration loaded: $config');

    compilerVersion = config[configVersion] ?? throwException(configVersion);
    generatedCodeOutputDir = config[configGenerateOutputDir] ??
        throwException(configGenerateOutputDir);
    projectName =
        config[configProjectName] ?? throwException(configProjectName);
    parentClass = UnrealClass(
      config[configParentClass]?[configName] ??
          throwException("$configParentClass::$configName"),
      config[configParentClass]?[configClassMacro] ??
          throwException("$configParentClass::$configClassMacro"),
      config[configParentClass]?[configPropertyMacro] ??
          throwException("$configParentClass::$configPropertyMacro"),
      config[configParentClass]?[configParentClass] ??
          throwException("$configParentClass::$configParentClass"),
    );

    childClass = UnrealClass(
        protobufMessageName,
        config[configChildClass]?[configClassMacro] ??
            throwException("$configChildClass::$configClassMacro"),
        config[configChildClass]?[configPropertyMacro] ??
            throwException("$configChildClass::$configPropertyMacro"),
        parentClass.name);
  }

  @override
  String toString() {
    return '''

Compiler version : $compilerVersion
generated code output dir : $generatedCodeOutputDir
unreal project name : $projectName
Generate Class Option:
  parent class :
    name : ${parentClass.name}
    class macro : ${parentClass.classMacro}
    property macro : ${parentClass.propertyMacro}
    parent class : ${parentClass.parentClass}

  child class :
    name : ${childClass.name}
    class macro : ${childClass.classMacro}
    property macro : ${childClass.propertyMacro}
    parent class : ${childClass.parentClass}

''';
  }
}
