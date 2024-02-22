using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MvcWork.Models
{
    public class FileUserService
    {
        private readonly string _filePath;
        private bool _adminExists;

        public FileUserService(string filePath)
        {
            _filePath = filePath;
            _adminExists = CheckAdminExists();
            EnsureFileExists();
        }

        private bool CheckAdminExists()
        {
            if (!File.Exists(_filePath))
            {
                return false;
            }

            foreach (var line in File.ReadLines(_filePath))
            {
                var userProps = line.Split('#');
                if (userProps.Length > 6 && bool.TryParse(userProps[5], out bool isAdmin) && isAdmin && bool.TryParse(userProps[6], out bool isActive) && isActive)
                {
                    return true;
                }
            }

            return false;
        }


        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }
        }

        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();

            foreach (var line in File.ReadLines(_filePath))
            {
                var userProps = line.Split('#');
                var user = new UserModel
                {
                    UserId = int.Parse(userProps[0]),
                    FirstName = userProps[1],
                    LastName = userProps[2],
                    MailAddress = userProps[3],
                    Password = userProps[4],
                    IsAdmin = bool.Parse(userProps[5]),
                    IsActive = bool.Parse(userProps[6]) 
                };
                users.Add(user);
            }

            return users;
        }

       

        public void AddUser(UserModel user)
        {
            var users = GetAllUsers();
            int maxUserId = users.Count > 0 ? users.Max(u => u.UserId) : 0; 
            user.UserId = maxUserId + 1; 
            user.IsActive = true; 

            if (!_adminExists)
            {
                user.IsAdmin = true;
                _adminExists = true;
            }

            string line = $"{user.UserId}#{user.FirstName}#{user.LastName}#{user.MailAddress}#{user.Password}#{user.IsAdmin}#{user.IsActive}###";
            File.AppendAllText(_filePath, line + Environment.NewLine);
        }

        public UserModel GetUserById(int id)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => u.UserId == id);
        }

        public void UpdateUser(UserModel updatedUser)
        {
            var users = GetAllUsers();
            var existingUser = users.FirstOrDefault(u => u.UserId == updatedUser.UserId);

            if (existingUser != null)
            {
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.MailAddress = updatedUser.MailAddress;
                existingUser.Password = updatedUser.Password;
                existingUser.IsActive = updatedUser.IsActive;

                // Dosyaya güncellenmiş kullanıcıları yaz
                WriteUsersToFile(users);
            }
            else
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }
        }

        private void WriteUsersToFile(List<UserModel> users)
        {
            // Tüm kullanıcıları dosyaya yaz
            File.WriteAllLines(_filePath, users.Select(user =>
                $"{user.UserId}#{user.FirstName}#{user.LastName}#{user.MailAddress}#{user.Password}#{user.IsAdmin}#{user.IsActive}###"));
        }
        public void DeleteUser(int userId)
        {
            var users = GetAllUsers();
            var userToDelete = users.FirstOrDefault(u => u.UserId == userId);

            if (userToDelete != null)
            {
                userToDelete.IsActive = false; // Kullanıcıyı pasif hale getir

                // Dosyaya güncellenmiş kullanıcıları yaz
                WriteUsersToFile(users);
            }
            else
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }
        }

        public int GetUserIdByMailAddress(string mailAddress)
        {
            var users = GetAllUsers();
            var user = users.FirstOrDefault(u => u.MailAddress == mailAddress);
            return user != null ? user.UserId : -1;
        }

    }
}
