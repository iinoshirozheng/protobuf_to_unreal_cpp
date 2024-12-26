import 'dart:io';
import 'package:args/args.dart';
import "parser/protobuf_parser.dart";
import 'generator/unreal_object_generator.dart';
import 'generator/generator_option.dart';

const version = "1.0.0";

void main(List<String> arguments) {
  String executableDir =
      Directory(File(Platform.resolvedExecutable).parent.path).path;
  print('Executable file Directory: $executableDir');
  print('Current working directory: ${Directory.current.path}');

  final parser = ArgParser()
    ..addOption('input', abbr: 'i', help: 'Input file path');

  ArgResults argResults;
  try {
    argResults = parser.parse(arguments);
  } catch (e) {
    print('Error: $e\n');
    print('Usage: dart run bin/main.dart -i <input_file_path>');
    exit(1);
  }

  final inputFilePath = argResults['input'];

  if (inputFilePath == null) {
    print('Error: Input file path is required.');
    print('Usage: dart run bin/main.dart -i <input_file_path>');
    exit(1);
  }

  try {
    var protoContent = File(inputFilePath).readAsStringSync();
    final protoParser = UnrealProtobufParser(protoContent)
      ..parsePackageNameSpace()
      ..parseMessages()
      ..parseEnums();

    final genOption = GenerateOption(version);
    final outputDirPath = genOption.generatedCodeOutputDir;

    // Check if output directory exists, if not, create it
    Directory('$executableDir/$outputDirPath').createSync(recursive: true);

    final objectGenerator =
        UnrealObjectGenerator(inputFilePath, protoParser, version)
          ..recordProtoName()
          ..generateEnumsCode()
          ..generateClassesCode();

    final generatedHeaderCode = objectGenerator.getGeneratedUnrealHeaderCode;

    final outputHeaderFileName =
        'Protobuf_${objectGenerator.getProtoFileName}.h';

    final outputHeaderFilePath =
        '$executableDir/$outputDirPath/$outputHeaderFileName';

    File(outputHeaderFilePath).writeAsStringSync(generatedHeaderCode);
    print('Generated .h code has been written to: $outputHeaderFilePath');

    if (objectGenerator.bufferCpp.isNotEmpty) {
      final outputSourceFileName =
          'Protobuf_${objectGenerator.getProtoFileName}.cpp';
      final outputSourceFilePath =
          '$executableDir/$outputDirPath/$outputSourceFileName';

      final generatedSourceCode = '#include "$outputHeaderFileName"' +
          "\n" +
          objectGenerator.getGeneratedUnrealSourceCode;
      File(outputSourceFilePath).writeAsStringSync(generatedSourceCode);
      print('Generated .cpp code has been written to: $outputSourceFilePath');
    }
  } catch (e) {
    print('Error: $e');
    exit(1);
  }
}
