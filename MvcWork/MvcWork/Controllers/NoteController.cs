using Microsoft.AspNetCore.Mvc;
using MvcWork.Models;
using System;
using System.Security.Claims;

namespace MvcWork.Controllers
{
    public class NoteController : Controller
    {
        private readonly FileNoteService _noteService;
        private readonly FileUserService _userService;

        public NoteController(FileNoteService noteService, FileUserService userService)
        {
            _noteService = noteService;
            _userService = userService;
        }

        public IActionResult NoteAdd(string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }


        [HttpPost]
        public IActionResult NoteAdd(NoteModel note)
        {

           
            if (note.UserId != 0)
            {
                var user = _userService.GetUserById(note.UserId);
                if (user != null && !user.IsAdmin)
                {
                    _noteService.AddNote(note);
                    return RedirectToAction("Index", new RouteValueDictionary(
   new { controller = "User", action = "Index", userId = note.UserId }));
                }
            }
            return RedirectToAction("Index", "Home"); // Giriş başarısız ise ana sayfaya yönlendir

        }

        public IActionResult NoteList(string userId)
        {
            
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userService.GetUserById(int.Parse(userId));
                if (user != null && !user.IsAdmin)
                {
                    var notes = _noteService.GetNotesByUserId(int.Parse(userId));
                    return View(notes);
                }
            }
            return RedirectToAction("Index", "Home"); // Kullanıcı hatalıysa ana sayfaya yönlendir
        }

        [HttpGet]
        public IActionResult NoteEdit(int noteId)
        {
            var note = _noteService.GetNoteById(noteId);
            if (note == null)
                return NotFound();

            return View(note);
        }

        [HttpPost]
        public IActionResult NoteEdit(NoteModel updatedNote)
        {
            _noteService.UpdateNote(updatedNote);
            return RedirectToAction("NoteList", new RouteValueDictionary(
   new { controller = "Note", action = "NoteList", userId = updatedNote.UserId }));
        }

        [HttpPost]
        public IActionResult NoteDelete(int noteId, int userId)
        {
            _noteService.DeleteNoteById(noteId);
            return RedirectToAction("NoteList", new RouteValueDictionary(
  new { controller = "Note", action = "NoteList", userId = userId }));
        }

    }
}
