using WasmLib.FileFormat;

namespace WasmLib.Decompilation.Intermediate.Instructions
{
    public class DropInstruction : IntermediateInstruction
    {
        public override ValueKind[] PopTypes => new[] {ValueKind.Any};
        public override ValueKind[] PushTypes => new ValueKind[0];

        public override bool CanInline => false; // not supposed to output anything

        public override string OperationStringFormat => string.Empty;
        public override string Comment => "drop {0}"; 
    }
}