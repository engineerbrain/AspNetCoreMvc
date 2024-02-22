using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MvcWork.Models
{
    public class FileNoteService
    {
        private readonly string _filePath;
        private readonly FileUserService _userService;

        public FileNoteService(string filePath, FileUserService userService)
        {
            _filePath = filePath;
            _userService = userService;
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }
        }


        public List<NoteModel> GetAllNotes()
        {
            List<NoteModel> notes = new List<NoteModel>();

            if (File.Exists(_filePath))
            {
                string[] lines = File.ReadAllLines(_filePath);
                foreach (var line in lines)
                {
                    string[] parts = line.Split('#');
                    if (parts.Length == 4)
                    {
                        notes.Add(new NoteModel
                        {
                            UserId = int.Parse(parts[0]),
                            NoteId = int.Parse(parts[1]),
                            NoteDescription = parts[2],
                            NoteDate = DateTime.Parse(parts[3])
                        });
                    }
                }
            }

            return notes;
        }
        public void AddNote(NoteModel note)
        {  
                var user = _userService.GetUserById(note.UserId);
                if (user != null && !user.IsAdmin)
                {                
                    var notes = GetAllNotes();
                    int maxNoteId = notes.Count > 0 ? notes.Max(n => n.NoteId) : 0;
                    note.NoteId = maxNoteId + 1;
                    note.NoteDate = DateTime.Now;

                    // Dosyaya notun eklenmesi işlemi burada gerçekleştirilir
                    string line = $"{note.UserId}#{note.NoteId}#{note.NoteDescription}#{note.NoteDate.ToString("yyyy-MM-dd HH:mm:ss")}###";
                    File.AppendAllText(_filePath, line + Environment.NewLine);
                }
            
        }

        public List<NoteModel> GetNotesByUserId(int userId)
        {
            var allNotes = GetAllNotes();
            return allNotes.Where(n => n.UserId == userId).ToList();
        }

        public NoteModel GetNoteById(int noteId)
        {
            var allNotes = GetAllNotes();
            return allNotes.FirstOrDefault(n => n.NoteId == noteId);
        }

        public void UpdateNote(NoteModel updatedNote)
        {
            var allNotes = GetAllNotes();
            var existingNote = allNotes.FirstOrDefault(n => n.NoteId == updatedNote.NoteId);

            if (existingNote != null)
            {
                existingNote.NoteDescription = updatedNote.NoteDescription;
                existingNote.NoteDate = updatedNote.NoteDate;

              
                WriteNotesToFile(allNotes);
            }
            else
            {
                throw new InvalidOperationException("Note not found.");
            }
        }

        private void WriteNotesToFile(List<NoteModel> notes)
        {
            
            File.WriteAllLines(_filePath, notes.Select(note =>
                $"{note.UserId}#{note.NoteId}#{note.NoteDescription}#{note.NoteDate.ToString("yyyy-MM-dd HH:mm:ss")}###"));
        }

        public void DeleteNoteById(int noteId)
        {
            var allNotes = GetAllNotes();
            var noteToDelete = allNotes.FirstOrDefault(n => n.NoteId == noteId);

            if (noteToDelete != null)
            {
                allNotes.Remove(noteToDelete);
                WriteNotesToFile(allNotes);
            }
            else
            {
                throw new InvalidOperationException("Not bulunamadı.");
            }
        }

    }
}
