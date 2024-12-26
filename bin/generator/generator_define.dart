const String configVersion = "version";
const String configGenerateOutputDir = "ue_generator_code_output_dir";
const String configParentClass = "parent_class";
const String configChildClass = "child_class";
const String configName = "name";
const String configClassMacro = "class_macro";
const String configPropertyMacro = "property_macro";
const String configProjectName = "ue_project_name";
const String configYamlPath = 'settings/config.yaml';
const String configMacro = 'macro';
const String configEnumClass = 'enum_class';

const String protobufMessageName = "#ProtobufMessageName#";
const String defaultYamlFile = '''
# Description: Settings for the application
# This is the configuration file for the Compiler, used to set the output path and class parameters (if deleted, one will be generated in the executable file directory)
# Note that each level must be two spaces, otherwise it will not be read!!!!

$configVersion: "1.0.0"
$configGenerateOutputDir: ""
$configProjectName: "SGF"

$configParentClass:
  # Name: Class name, #Name# is the name read from the Proto file
  $configName: "UObject"
  # parent_class_name: Name of the inherited parent class
  $configParentClass: "UObject"
  $configClassMacro: "UCLASS()"
  $configPropertyMacro: "UPROPERTY()"

$configChildClass:
  $configClassMacro: "UCLASS(BlueprintType, Blueprintable)"
  $configPropertyMacro: "UPROPERTY(BlueprintReadWrite)"

$configEnumClass:
  $configMacro: "UENUM(BlueprintType)"
''';
