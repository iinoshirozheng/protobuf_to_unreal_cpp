import 'dart:io';
import "parser/protobuf_parser.dart";
import 'generater/unreal_object_generator.dart';

const String version = '0.0.1';
// import 'package:args/args.dart';
// ArgParser buildParser() {
//   return ArgParser()
//     ..addFlag(
//       'help',
//       abbr: 'h',
//       negatable: false,
//       help: 'Print this usage information.',
//     )
//     ..addFlag(
//       'verbose',
//       abbr: 'v',
//       negatable: false,
//       help: 'Show additional command output.',
//     )
//     ..addFlag(
//       'version',
//       negatable: false,
//       help: 'Print the tool version.',
//     );
// }
//
// void printUsage(ArgParser argParser) {
//   print('Usage: dart protobuf_to_unreal_cpp.dart <flags> [arguments]');
//   print(argParser.usage);
// }

// void main(List<String> arguments) {
//   final ArgParser argParser = buildParser();
//   try {
//     final ArgResults results = argParser.parse(arguments);
//     bool verbose = false;
//
//     // Process the parsed arguments.
//     if (results.wasParsed('help')) {
//       printUsage(argParser);
//       return;
//     }
//     if (results.wasParsed('version')) {
//       print('protobuf_to_unreal_cpp version: $version');
//       return;
//     }
//     if (results.wasParsed('verbose')) {
//       verbose = true;
//     }
//
//     // Act on the arguments provided.
//     print('Positional arguments: ${results.rest}');
//     if (verbose) {
//       print('[VERBOSE] All arguments: ${results.arguments}');
//     }
//   } on FormatException catch (e) {
//     // Print usage information if an invalid argument was provided.
//     print(e.message);
//     print('');
//     printUsage(argParser);
//   }
// }
Future<String> readProtoFile(String filePath) async {
  return await File(filePath).readAsString();
}

Future<void> main() async {
  final protoFilePath = 'FTGTestingProtobuf.proto';
  final protoContent = await readProtoFile(protoFilePath);
  final parser = UnrealProtobufParser(protoContent);
  final messages = parser.parse();
  print(messages);
  final objectGenerator = UnrealObjectGenerator(protoFilePath);
  objectGenerator.generateMessageCode(messages);
  objectGenerator.generateMessageCode(messages);
  print(objectGenerator.getGeneratedUnrealCode);
}
