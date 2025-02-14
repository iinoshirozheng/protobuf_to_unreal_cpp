import 'package:yaml/yaml.dart';
import 'generator_define.dart';
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
      '$configName configuration error, please check config.yaml and ensure the parameter format is as follows:\n$defaultYamlFile');

  late String generatedCodeOutputDir;
  late String compilerVersion;
  late String projectName;
  late UnrealClass parentClass;
  late UnrealClass childClass;
  late String enumClassMacro;
  GenerateOption(currentVersion) {
    // Load the configuration file.
    final File configFile = File(
        '${Directory(File(Platform.resolvedExecutable).parent.path).path}/$configYamlPath');
    print('configFile: $configFile');
    if (!configFile.existsSync()) {
      print(
          'Configuration file not found. Automatically generating config.yaml file (in the same directory as the executable)');
      configFile.writeAsStringSync(defaultYamlFile);
    }
    final String configString = configFile.readAsStringSync();
    final YamlMap config = loadYaml(configString);

    compilerVersion = config[configVersion] ?? throwException(configVersion);
    if (compilerVersion != currentVersion) {
      throw Exception(
          'Configuration file version error. Please check if the version in config.yaml is correct.\nIf unsure, delete the configuration file and a new config.yaml with the latest version will be automatically generated.');
    }
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

    enumClassMacro = config[configEnumClass]?[configMacro] ??
        throwException("$configEnumClass::$configMacro");
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

  enum class :
    enum class macro : $enumClassMacro
''';
  }
}
