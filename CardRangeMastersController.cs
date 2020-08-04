using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using addNewCred.Models;
using System.Data.SqlClient;


namespace addNewCred.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardRangeMastersController : ControllerBase
    {
        private readonly ccmpContext _context;

        public CardRangeMastersController(ccmpContext context)
        {
            _context = context;
        }

        // GET: api/CardRangeMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardRangeMaster>>> GetCardRangeMaster()
        {
            return await _context.CardRangeMaster.ToListAsync();
        }

        //// GET: api/CardRangeMasters/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CardRangeMaster>> GetCardRangeMaster(int id)
        //{
        //    var cardRangeMaster = await _context.CardRangeMaster.FindAsync(id);

        //    if (cardRangeMaster == null)
        //    {
        //        return NotFound();
        //    }

        //    return cardRangeMaster;
        //}

        //// PUT: api/CardRangeMasters/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCardRangeMaster(int id, CardRangeMaster cardRangeMaster)
        //{
        //    if (id != cardRangeMaster.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(cardRangeMaster).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CardRangeMasterExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/CardRangeMasters
        [HttpPost]
        public async Task<ActionResult<CardRangeMaster>> PostCardRangeMaster(int f_code, string cstart, string cend, string status, DateTime cdate, DateTime edate)
        {
            // _context.CardRangeMaster.Add(cardRangeMaster);
            // string query = "IF EXISTS(SELECT 1 FROM table WHERE @myValueLo <= ExistingRangeEnd AND @myValueHi >= ExistingRangeStart)";

            //await _context.SaveChangesAsync();

            //Int32 start = Int32.Parse(cardRangeMaster.CardRangeStart);
            //int  start = int.Parse(cardRangeMaster.CardRangeStart);
            // Int32 end = Int32.Parse(cardRangeMaster.CardRangeEnd);
            //int code = 0;
            int start = int.Parse(cstart);
            int end = int.Parse(cend);

            if (start > end)
                return BadRequest("Invalid start/end range provided");
            else
            {
                var query = "Select * from Card_Range_Master where Facility_code is f_code";
                var query1 = "(Select * from Card_Range_Master where convert(int , Card_Range_Start) > start and convert(int , Card_Range_Start)> end)";
                var query2 = "(Select * from Card_Range_Master where convert(int , Card_Range_End) < end and convert(int , Card_Range_End) < start)";

                SqlConnection con = new SqlConnection("Server=10.21.52.198;Database=ccmp;User ID=****;Password=******");
                con.Open();

                SqlCommand cmd1 = new SqlCommand(query1, con);

                SqlCommand cmd2 = new SqlCommand(query2, con);

                SqlCommand cmd3 = new SqlCommand(query, con);

                Console.WriteLine(start);
                Console.WriteLine(end);

                cmd3.ExecuteNonQuery();
                if (cmd3 != null)
                {
                   // try
                    //{
                        cmd1.ExecuteNonQuery();
                        if (cmd1 != null)
                        return BadRequest("Invalid start/end range provided");

                   // code = 2;
                   // }
                   // catch (Exception e)//throw exception
                   // {
                    //    Environment.Exit(code);
                    //    Console.WriteLine("check input");
                    //}
                    //try
                    //{
                        cmd2.ExecuteNonQuery();
                        if (cmd2 != null)
                        return BadRequest("Invalid start/end range provided");
                    //        code = 3;
                    //}
                    //catch (Exception e)//throw exception
                    //{
                    //    Environment.Exit(code);
                    //    Console.WriteLine("check input");
                    //}


                }

                if (cmd3 == null)
                {
                    CardRangeMaster cardRangeMaster = new CardRangeMaster();
                    _context.CardRangeMaster.Add(cardRangeMaster);
                    await _context.SaveChangesAsync();

                    var cardrange = CreatedAtAction("GetCardRangeMaster", new { id = cardRangeMaster.Id }, cardRangeMaster);

                    return cardrange;
                }
                else if (cmd3 != null && cmd1 == null && cmd2 == null)
                {
                    CardRangeMaster cardRangeMaster = new CardRangeMaster();
                    _context.CardRangeMaster.Add(cardRangeMaster);
                    await _context.SaveChangesAsync();

                    var cardrange = CreatedAtAction("GetCardRangeMaster", new { id = cardRangeMaster.Id }, cardRangeMaster);

                    return cardrange;
                }
                else
                    return BadRequest("Invalid start/end range provided");

            }

            //string query =
            //    "Insert into Card_Range_Master where fac_code is Facility_Code and start < end " +
            //    "[ where Card_Range_Start > start and Card_Range_Start > end ] and [ where Card_Range_End < end and Card_Range_End < start]";

            
            //CardRangeMaster cardRangeMaster = new CardRangeMaster();
            //    _context.CardRangeMaster.Add(cardRangeMaster);
            //await _context.SaveChangesAsync();

            //var cardrange = CreatedAtAction("GetCardRangeMaster", new { id = cardRangeMaster.Id }, cardRangeMaster);

            //return cardrange;

            //DECLARE @i int = CardRangeStart
            //    while @i < CardRangeEnd
            //    BEGIN 
            //        Set @i= i +1
            //    Insert 


        }

        // DELETE: api/CardRangeMasters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CardRangeMaster>> DeleteCardRangeMaster(int id)
        {
            var cardRangeMaster = await _context.CardRangeMaster.FindAsync(id);
            if (cardRangeMaster == null)
            {
                return NotFound();
            }

            _context.CardRangeMaster.Remove(cardRangeMaster);
            await _context.SaveChangesAsync();

            return cardRangeMaster;
        }

        private bool CardRangeMasterExists(int id)
        {
            return _context.CardRangeMaster.Any(e => e.Id == id);
        }
    }
}
