using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_Igor_Gustavo.Data;
using P1_Igor_Gustavo.Models;

namespace P1_Igor_Gustavo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly P1_Igor_GustavoContext _context;

        public VendasController(P1_Igor_GustavoContext context)
        {
            _context = context;
        }

        // GET: api/Vendas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> GetVenda()
        {
          if (_context.Venda == null)
          {
              return NotFound();
          }
            return await _context.Venda.Include(v => v.Cliente).Include(v => v.Produto).ToListAsync();
        }

        // GET: api/Vendas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetVenda(int id)
        {
          if (_context.Venda == null)
          {
              return NotFound();
          }
            var venda = await _context.Venda.Include(v => v.Cliente).Include(v => v.Produto).Where(v => v.Id == id).FirstOrDefaultAsync();

            if (venda == null)
            {
                return NotFound();
            }

            return venda;
        }

        // PUT: api/Vendas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenda(int id, Venda venda)
        {
            if (id != venda.Id)
            {
                return BadRequest();
            }

            _context.Entry(venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(id))
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

        // POST: api/Vendas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/Vendas")]
        public async Task<ActionResult<Venda>> PostVenda(int clienteId, int produtoId, int quantidade)
        {
            if (_context.Venda == null || _context.Estoque == null || _context.Produto == null)
            {
                return Problem("Entity set 'P1_Igor_Gustavo.Venda'  is null.");
            }

            if (quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que '0'.");
            }

            Estoque? estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.Produto.Id == produtoId);

            if (estoque == null)
            {
                return NotFound("Produto não encontrado no estoque.");
            }

            Cliente? cliente = await _context.Cliente.FindAsync(clienteId);

            if (cliente == null)
            {
                return NotFound("O cliente não foi encontrado.");
            }

            Produto? produto = await _context.Produto.FindAsync(produtoId);

            if (produto == null)
            {
                return NotFound("Nenhum produto foi encontrado.");
            }

            Venda venda = new Venda()
            {
                Cliente = cliente,
                Produto = produto,
                Quantidade = quantidade
            };

            if (quantidade > estoque.Quantidade)
            {
                return BadRequest("O número de produtos excede a quantidade armazenada em estoque.");
            }

            estoque.DecrementarQuantidade(quantidade);

            _context.Estoque.Update(estoque);
            _context.Venda.Add(venda);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Id }, venda);
        }


        // DELETE: api/Vendas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenda(int id)
        {
            if (_context.Venda == null)
            {
                return NotFound();
            }
            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            _context.Venda.Remove(venda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendaExists(int id)
        {
            return (_context.Venda?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
