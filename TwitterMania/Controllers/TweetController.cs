using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterMania.DataAccess;
using TwitterMania.Model;

namespace TwitterMania.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TweetController : ControllerBase
    {

        private ApplicationDbContext _context;

        public TweetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Tweet/{id}")]
        public TweetModel GetTweetById(int id) 
        {
            return _context.Tweet.FirstOrDefault(tweet => tweet.ID == id);
        }

        [HttpGet]
        [Route("LastTweets/{userId}")]
        public List<TweetModel> GetLastTweetsFromUser(int userId) 
        {
            return _context.Tweet.Where(s => s.UserId == userId)
                .OrderByDescending(tweet => tweet.DateTime)
                .Take(10).ToList();
        }

        [HttpGet("UserId/{username}")]
        public int GetUserId(string username)
        {
            return _context.User.FirstOrDefault(user => user.Username == username).ID;
        }

        [HttpGet]
        [Route("GetUsers")]
        public List<UserModel> GetAllUsers()
        {
            return _context.User
                .OrderByDescending(t => t.ID)
                .Take(100).ToList();
        }

        [HttpPost("AddTweet")]
        public IActionResult PostTweet(TweetModel tweet) 
        {
            //tweet.DateTime = new DateTime();
            _context.Tweet.Add(tweet);
            _context.SaveChanges();

            return Ok(tweet);
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult PostAddUser(UserModel user) 
        {
            if (user.Username == null || user.Password == null || user.Email == null)
                return BadRequest();

            var user1 = _context.User.FirstOrDefault(korisnik => user.Username == korisnik.Username);
            var user2 = _context.User.FirstOrDefault(korisnik => user.Email == korisnik.Email);
            if (user1 != null || user2 != null)
                return BadRequest();
            if (user.UserType == null)
                user.UserType = "User";

            _context.User.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpGet]
        [Route("User/{id}")]
        public UserModel GetUserById(int id) {

            return _context.User.FirstOrDefault(user => user.ID == id);        
        }

        [HttpGet("UserBio/{id}")]
        public string GetUserBio(int id) {

            return _context.User.Find(id).Bio;
        }

        [HttpPatch("EditUserBio/{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] JsonPatchDocument<UserModel> patchDoc) 
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }


        [HttpPatch("EditTweet/{id}")]
        public async Task<IActionResult> PatchTweet(int id, [FromBody] JsonPatchDocument<TweetModel> patchDoc) 
        {
            if (patchDoc == null) 
            {
                return BadRequest();
            }
            var tweet = await _context.Tweet.FindAsync(id);

            if (tweet == null) 
            {
                return NotFound();
            }

            patchDoc.ApplyTo(tweet);
            await _context.SaveChangesAsync();

            return Ok(tweet);
        }
        [HttpPatch("EditUserProfile/{id}")]
        public async Task<IActionResult> PatchUserProfile(int id, JsonPatchDocument<UserModel> patchUser) 
        {
            if (patchUser == null)
                return BadRequest();

            var user = await _context.User.FindAsync(id);

            if (user == null)
                return NotFound();

            patchUser.ApplyTo(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete]
        [Route("DeleteTweet/{id}")]
        public async Task<IActionResult> DeleteTweetAsync (int id) 
        {
            var tweetModel = await _context.Tweet.FindAsync(id);
            if (tweetModel == null)
            {
                return NotFound();
            }

            _context.Tweet.Remove(tweetModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TweetExists(int id) 
        {
            return _context.Tweet.Any(t => t.ID == id);
        }

    }
}
