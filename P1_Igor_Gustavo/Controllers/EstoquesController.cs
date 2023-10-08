using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_Igor_Gustavo.Data;
using P1_Igor_Gustavo.Models;

namespace P1_Igor_Gustavo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoquesController : ControllerBase
    {
        private readonly P1_Igor_GustavoContext _context;

        public EstoquesController(P1_Igor_GustavoContext context)
        {
            _context = context;
        }

        // GET: api/Estoques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estoque>>> GetEstoque()
        {
          if (_context.Estoque == null)
          {
              return NotFound();
          }
            return await _context.Estoque.Include(e => e.Produto).ToListAsync();
        }

        // GET: api/Estoques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estoque>> GetEstoque(int id)
        {
          if (_context.Estoque == null)
          {
              return NotFound();
          }
            var estoque = await _context.Estoque.Include(e => e.Produto).Where(e => e.Id == id).FirstOrDefaultAsync();

            if (estoque == null)
            {
                return NotFound();
            }

            return estoque;
        }

        // PUT: api/Estoques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstoque(int id, Estoque estoque)
        {
            if (id != estoque.Id)
            {
                return BadRequest();
            }

            _context.Entry(estoque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstoqueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Estoques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estoque>> PostEstoque(string nome, int produtoId, int quantidade)
        {
            if (_context.Estoque == null || _context.Produto == null)
            {
                return Problem("Entity set 'ProvaP1Context.Estoque'  is null.");
            }

            Produto? produto = await _context.Produto.FindAsync(produtoId);

            if (produto == null)
            {
                return NotFound($"O produto com o ID: {produtoId} não foi encontrado.");
            }

            Estoque estoque = new Estoque()
            {
                Nome = nome,
                Produto = produto,
                Quantidade = quantidade
            };

            _context.Estoque.Add(estoque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstoque", new { id = estoque.Id }, estoque);
        }

        // DELETE: api/Estoques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstoque(int id)
        {
            if (_context.Estoque == null)
            {
                return NotFound();
            }
            var estoque = await _context.Estoque.FindAsync(id);
            if (estoque == null)
            {
                return NotFound();
            }

            _context.Estoque.Remove(estoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Estoques/remove/5
        //Remover protudo do estoque
        [HttpPut("remove/{id}")]
        public async Task<IActionResult> RemoverEstoque(int id, int quantidade)
        {
            if (_context.Estoque == null) { return NotFound(); }

            Estoque? estoque = await _context.Estoque.FindAsync(id);

            if (estoque == null)
            {
                return NotFound(id);
            }

            if (estoque.Quantidade < quantidade)
            {
                return BadRequest("Quantidade a ser removida é maior do que a quantidade em estoque.");
            }

            _context.Entry(estoque).State = EntityState.Modified;

            estoque.Quantidade -= quantidade;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EstoqueExists(int id)
        {
            return (_context.Estoque?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //Add produto
        [HttpPut("/add/{id}")]
        public async Task<IActionResult> AdicionarEstoque(int id, int quantidade)
        {
            if (_context.Estoque == null) { return NotFound(); }

            Estoque? estoque = await _context.Estoque.FindAsync(id);

            if (estoque == null)
            {
                return NotFound(id);
            }

            _context.Entry(estoque).State = EntityState.Modified;

            estoque.Quantidade += quantidade;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
