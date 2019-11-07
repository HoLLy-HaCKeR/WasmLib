using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WasmLib.Decompilation.Intermediate;
using WasmLib.FileFormat;

namespace WasmLib.Decompilation
{
    public class IntermediateRepresentationDecompiler : IDecompiler
    {
        public WasmFile WasmFile { get; }

        public IntermediateRepresentationDecompiler(WasmFile wasmFile)
        {
            WasmFile = wasmFile;
        }

        public void DecompileFunction(StreamWriter output, int functionIndex)
        {
            FunctionBody body = WasmFile.FunctionBodies[functionIndex];
            FunctionSignature signature = WasmFile.FunctionTypes[WasmFile.Functions[functionIndex]];
            
            // get IR
            var context = new IntermediateContext(body, signature, WasmFile, output);
            List<IntermediateInstruction> instructions = new IntermediateConverter(WasmFile, body).Convert();

            output.Write(signature.ToString($"fun_{functionIndex:X8}"));
            output.WriteLine(" {");

            // write all IR while simulating the stack
            foreach (IntermediateInstruction instruction in instructions) {
                instruction.Handle(ref context);
                Debug.Assert(!context.EndOfBlock, "Encountered EndOfBlock instruction when not in block");
            }
            
            // write return value, if needed
            if (signature.ReturnParameter.Length != 0) {
                new FakeReturnInstruction().Handle(ref context);
            }
            
            output.WriteLine("}");

            if (context.Indentation != 0) {
                throw new Exception("Function body has unbalanced indentation");
            }

            if (context.Stack.Count != 0) {
                // throw new Exception($"Unbalanced stack, found {context.Stack.Count} remaining values");
                Console.WriteLine($"Unbalanced stack, found {context.Stack.Count} remaining values");
            }
            
            output.WriteLine();
        }
    }
}