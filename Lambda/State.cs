namespace Lambda.GenH30
{
    public abstract class State
    {
        public abstract void PowerOn(ILambdaWorker worker);
        public abstract void Read(ILambdaWorker worker);
        public abstract void ReadInit(ILambdaWorker worker);
        public abstract void Failure();
    }
}