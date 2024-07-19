using System.Collections.Generic;
using Character;

public class CharacterCollection
{
    public List<CharacterBase> collection = new List<CharacterBase>();

    public void Add(CharacterBase character)
    {
        collection.Add(character);
    }

    public void Add(IEnumerable<CharacterBase> characterList)
    {
        collection.AddRange(characterList);
    }

    public void Clear()
    {
        collection.Clear();
    }
}