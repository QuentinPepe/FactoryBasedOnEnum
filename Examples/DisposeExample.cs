using System;
using System.IO;
using System.Text;

namespace FactoryBasedOnEnum.Examples
{
    public class DisposeExample
    {
        private readonly GenericFactory<CharacterType, IDisposableCharacter> _characterFactory = new GenericFactory<CharacterType, IDisposableCharacter>();

        public void Play()
        {
            IDisposableCharacter warrior = _characterFactory.GetInstance(CharacterType.Warrior);
            warrior.PerformAction();

            DisposeCharacter(warrior);
        }

        private void DisposeCharacter(IDisposableCharacter character)
        {
            character.Dispose();
        }

        public void EndGame()
        {
            _characterFactory.DisposeInstances();
        }
    }


    internal interface IDisposableCharacter : IDisposable
    {
        void PerformAction();
    }

    [EnumAssociated(typeof(CharacterType), CharacterType.Mage)]
    public class Mage : IDisposableCharacter
    {
        private readonly FileStream _fileStream = new FileStream("mage_log.txt", FileMode.Create);

        public void PerformAction()
        {
            byte[] info = new UTF8Encoding(true).GetBytes("Mage performed an action\n");
            _fileStream.Write(info, 0, info.Length);
        }

        public void Dispose()
        {
            _fileStream.Close();
        }
    }
}