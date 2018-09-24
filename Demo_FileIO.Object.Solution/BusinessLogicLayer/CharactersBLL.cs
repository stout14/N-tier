﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Demo_FileIO
{
    public class CharactersBLL
    {
        IDataService _dataService;
        List<Character> _characters;

        public IEnumerable<Character> GetCharacters(out bool success, out string message)
        {
            _characters = null;
            success = false;
            message = "";
            try
            {
                _dataService = new CsvDataService();
                _characters = _dataService.ReadAll() as List<Character>;
                if (_characters.Count > 0)
                {
                    success = true;
                }
                else
                {
                    message = "It appears there is no data in the file.";
                }
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return _characters;
        }

        public Character GetCharacterById(int id)
        {
            return _characters.FirstOrDefault(c => c.Id == id);
        }

        public void AddCharacter(Character character, out bool success, out string message)
        {
            success = false;
            message = "";
            try
            {
                _dataService = new CsvDataService();
                _characters = _dataService.ReadAll() as List<Character>;
                character.Id = MaximumCurrentId() + 1;
                _characters.Add(character);
                _dataService.WriteAll(_characters);
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
        }

        public void UpdateCharacter(Character character)
        {
            _characters.RemoveAll(c => c.Id == character.Id);
            _characters.Add(character);
            _dataService.WriteAll(_characters);
        }

        public void DeleteCharacter(int id)
        {
            _characters.RemoveAll(c => c.Id == id);
            _dataService.WriteAll(_characters);
        }

        public CharactersBLL()
        {

        }

        private int MaximumCurrentId()
        {
            return _characters.Max(c => c.Id);
        }
    }
}
