using System.Collections.Generic;

public interface IComponentInputable
{
    public void Receive(Direction from, List<MonoPicture> picts);
}