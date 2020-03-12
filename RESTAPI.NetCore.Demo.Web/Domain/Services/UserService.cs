using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Newtonsoft.Json;
using RESTAPI.NetCore.Demo.Common.Contracts;
using RESTAPI.NetCore.Demo.Common.Models;
using RESTAPI.NetCore.Demo.Common.ViewModels;
using RESTAPI.NetCore.Demo.Web.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTAPI.NetCore.Demo.Web.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IFileService _fileService;
        public UserService(IRepository<User> userRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public async Task<List<UserViewModel>> Get(string name = "", int pageSize = 1, int pageNum = 1)
        {
            try
            {
                var users = await _userRepository.Get(u => u.FirstName.Contains(name) || u.LastName.Contains(name),
                                                        (pageNum - 1) * pageSize,
                                                        pageSize);

                return users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = new UserNameViewModel
                    {
                        Title = u.Title,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    },
                    BirthDay = u.BirthDay,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ImageUrl = _fileService.GetImgFileUrl(u.ImageId, FileType.imageThumb)
                }
                ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<UserViewModel> GetById(Guid id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    throw new Exception("The user does not exist");
                }

                return new UserViewModel
                {
                    Id = user.Id,
                    Name = new UserNameViewModel
                    {
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    },
                    BirthDay = user.BirthDay,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl = _fileService.GetImgFileUrl(user.ImageId, FileType.imageLarge)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserViewModel>> BulkGenerate(int count)
        {
            try
            {
                var names = JsonConvert.DeserializeObject<Names>(_fileService.ReadResourceFile("names"));
                var users = new List<User>();
                var usersReturn = new List<UserViewModel>();
                for (int i = 0; i < count; i++)
                {
                    var objectId = ObjectId.GenerateNewId().ToString();
                    var id = Guid.NewGuid();
                    var title = "Mr";
                    var firstName = PopFromArray(names.FirstNames);
                    var lastName = PopFromArray(names.LastNames);
                    var birthDay = GenerateRandomDay();
                    var email = $"{firstName}.{lastName}@example.com";
                    var phoneNum = GeneratePhoneNumber();
                    var imgId = new Random().Next(1, 10).ToString();

                    users.Add(new User
                    {
                        ObjectId = objectId,
                        Id = id,
                        Title = title,
                        FirstName = firstName,
                        LastName = lastName,
                        BirthDay = birthDay,
                        Email = email,
                        PhoneNumber = phoneNum,
                        ImageId = imgId
                    });

                    usersReturn.Add(new UserViewModel
                    {
                        Id = id,
                        Name = new UserNameViewModel
                        {
                            Title = title,
                            FirstName = firstName,
                            LastName = lastName
                        },
                        BirthDay = birthDay,
                        Email = email,
                        PhoneNumber = phoneNum,
                        ImageUrl = _fileService.GetImgFileUrl(imgId, FileType.imageThumb)
                    });
                }

                await _userRepository.BulkAdd(users);

                return usersReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UserViewModel> Update(User user)
        {
            try
            {
                if (user.Id == null)
                {
                    throw new Exception("The user id should not be null");
                }

                var userForUpdating = await _userRepository.GetById(user.Id);
                if (userForUpdating == null)
                {
                    throw new Exception("The user does not exist");
                }

                userForUpdating.Title = user.Title;
                userForUpdating.FirstName = user.FirstName;
                userForUpdating.LastName = user.LastName;
                userForUpdating.BirthDay = user.BirthDay;
                userForUpdating.Email = user.Email;
                userForUpdating.PhoneNumber = user.PhoneNumber;
                userForUpdating.ImageId = user.ImageId;

                await _userRepository.Update(userForUpdating);

                return new UserViewModel
                {
                    Id = userForUpdating.Id,
                    Name = new UserNameViewModel
                    {
                        Title = userForUpdating.Title,
                        FirstName = userForUpdating.FirstName,
                        LastName = userForUpdating.LastName
                    },
                    BirthDay = userForUpdating.BirthDay,
                    Email = userForUpdating.Email,
                    PhoneNumber = userForUpdating.PhoneNumber,
                    ImageUrl = _fileService.GetImgFileUrl(userForUpdating.ImageId, FileType.imageLarge)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    throw new Exception("The user does not exist");
                }

                await _userRepository.Delete(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        public async Task<UserViewModel> Add(User user)
        {

            try
            {
                user.Id = Guid.NewGuid();
                if (file != null)
                {
                    //user.ImageId = _fileService.SaveFile(file, FileType.image);
                    //user.ImageCompressedId = _fileService.SaveFile(file, FileType.image, true);
                }
                await _userRepository.Add(user);

                return new UserViewModel
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Title = user.Title,
                    BirthDay = user.BirthDay,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    //ImageUrl = _fileService.GetFileUrl(user.ImageId, FileType.image)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        private string PopFromArray(string[] array)
        {
            var random = new Random();
            var index = random.Next(0, array.Length - 1);
            return array[index];
        }

        private string GeneratePhoneNumber()
        {
            StringBuilder telNo = new StringBuilder();
            Random random = new Random();
            int number;

            telNo.Append("(");
            for (int i = 0; i <= 3; i++)
            {
                number = random.Next(0, 9);
                telNo = telNo.Append(number.ToString());
            }
            telNo.Append(")");
            for (int i = 0; i <= 4; i++)
            {
                number = random.Next(0, 9);
                telNo = telNo.Append(number.ToString());
            }
            telNo = telNo.Append("-");
            for (int i = 0; i <= 6; i++)
            {
                number = random.Next(0, 9);
                telNo = telNo.Append(number.ToString());
            }
            return telNo.ToString();
        }

        private DateTime GenerateRandomDay()
        {
            Random random = new Random();
            DateTime startDate = new DateTime(1920, 1, 1);
            int range = (DateTime.Today - startDate).Days;
            return startDate.AddDays(random.Next(range));
        }
    }
}
