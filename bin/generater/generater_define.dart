const String configVersion = "version";
const String configGenerateOutputDir = "ue_generator_code_output_dir";
const String configParentClass = "parent_class";
const String configChildClass = "child_class";
const String configName = "name";
const String configClassMacro = "class_macro";
const String configPropertyMacro = "property_macro";
const String configProjectName = "ue_project_name";
const String configYamlPath = 'config.yaml';
const String protobufMessageName = "#ProtobufMessageName#";
const String defaultYamlFile = '''
# Description: Settings for the application
# 此為Compiler的設定檔案，用來設定輸出的路徑，以及類別的參數設定 (如果砍掉會在執行檔案目錄生成一個)
# 注意每層必須是兩個空格，不然會讀不到!!!!

/////////////////////////////////////////////////////////////////////

$configVersion: 1.0.0
$configGenerateOutputDir: ""
$configProjectName: "SGF"

$configParentClass:
  # Name : 類別名稱 #Name# 為讀取的Proto檔案中的名稱
  $configName: "UProtobuf"
  # parent_class_name : 繼承的父類別名稱
  $configParentClass: "UObject"
  $configClassMacro: "UCLASS()"
  $configPropertyMacro: "UPROPERTY()"

$configChildClass:
  $configClassMacro: "UCLASS(BlueprintType, Blueprintable)"
  $configPropertyMacro: "UPROPERTY(BlueprintReadWrite)"

/////////////////////////////////////////////////////////////////////
''';
