using System.ComponentModel;
using WasmLib.FileFormat;
using WasmLib.FileFormat.Instructions;
using WasmLib.Utils;

namespace WasmLib.Decompilation.Intermediate
{
    public class TestOperationInstruction : IntermediateInstruction
    {
        public ValueKind Type { get; }
        public OperationKind Operation { get; }
        
        public TestOperationInstruction(in Instruction instruction)
        {
            (Type, Operation) = instruction.OpCode switch {
                OpCode.I32Eqz => (ValueKind.I32, OperationKind.Eqz),
                OpCode.I64Eqz => (ValueKind.I64, OperationKind.Eqz),
                _ => throw new WrongInstructionPassedException(instruction, nameof(VariableInstruction)),
            };
        }

        public override ValueKind[] PopTypes => new[] {Type};
        public override ValueKind[] PushTypes => new[] {ValueKind.I32};

        protected override string OperationStringFormat => $"{{0}} = {string.Format(EnumUtils.GetDescription(Operation), "{1}")}";

        public enum OperationKind
        {
            [Description("{0} == 0")] Eqz,
        }
    }
}