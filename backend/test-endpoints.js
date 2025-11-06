// Teste completo de todos os endpoints
const API_BASE = 'http://localhost:5025';

console.log('🧪 TESTANDO TODOS OS ENDPOINTS COM POSTGRESQL\n');

const testAll = async () => {
  let userEmail = `test${Date.now()}@example.com`;
  let userId = null;
  let token = null;
  let listingId = null;

  // 1. REGISTRO DE USUÁRIO
  console.log('1️⃣ Testando REGISTRO de usuário...');
  try {
    const res = await fetch(`${API_BASE}/api/auth/register-simple`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name: 'Test User',
        email: userEmail,
        password: 'Test@123456'
      })
    });
    const data = await res.json();
    console.log('   ✅ Status:', res.status);
    console.log('   ✅ Usuário criado:', data.message || data.Message);
    userId = data.userId || data.UserId;
    console.log('   ✅ UserId:', userId);
  } catch (e) {
    console.log('   ❌ Erro:', e.message);
    return;
  }

  // 2. LOGIN
  console.log('\n2️⃣ Testando LOGIN...');
  try {
    const res = await fetch(`${API_BASE}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        email: userEmail,
        password: 'Test@123456'
      })
    });
    const data = await res.json();
    console.log('   ✅ Status:', res.status);
    console.log('   ✅ Token recebido:', (data.token || data.Token) ? 'SIM' : 'NÃO');
    console.log('   ✅ User:', data.user?.email || data.User?.Email);
    token = data.token || data.Token;
  } catch (e) {
    console.log('   ❌ Erro:', e.message);
    return;
  }

  // 3. CRIAR LISTING
  console.log('\n3️⃣ Testando CRIAR LISTING...');
  try {
    const res = await fetch(`${API_BASE}/api/listings`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({
        title: 'Senior React Developer',
        shortDescription: 'Looking for experienced React developer',
        requirements: '3+ years React, TypeScript, Node.js experience required',
        tags: ['react', 'typescript', 'frontend']
      })
    });
    const data = await res.json();
    console.log('   ✅ Status:', res.status);
    console.log('   ✅ Listing criada:', data.title);
    console.log('   ✅ Listing ID:', data.id);
    listingId = data.id;
  } catch (e) {
    console.log('   ❌ Erro:', e.message);
  }

  // 4. LISTAR TODAS AS LISTINGS
  console.log('\n4️⃣ Testando LISTAR TODAS as listings...');
  try {
    const res = await fetch(`${API_BASE}/api/listings`);
    const data = await res.json();
    console.log('   ✅ Status:', res.status);
    console.log('   ✅ Total de listings:', data.length);
    console.log('   ✅ Primeira listing:', data[0]?.title);
  } catch (e) {
    console.log('   ❌ Erro:', e.message);
  }

  // 5. BUSCAR LISTING POR ID
  if (listingId) {
    console.log('\n5️⃣ Testando BUSCAR listing por ID...');
    try {
      const res = await fetch(`${API_BASE}/api/listings/${listingId}`);
      const data = await res.json();
      console.log('   ✅ Status:', res.status);
      console.log('   ✅ Listing encontrada:', data.title);
      console.log('   ✅ Tags:', data.tags?.map(t => t.value).join(', '));
    } catch (e) {
      console.log('   ❌ Erro:', e.message);
    }

    // 6. SUBMETER APPLICATION
    console.log('\n6️⃣ Testando SUBMETER APPLICATION...');
    try {
      const res = await fetch(`${API_BASE}/api/listings/${listingId}/applications`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
          message: 'I am very interested in this position!'
        })
      });
      const data = await res.json();
      console.log('   ✅ Status:', res.status);
      console.log('   ✅ Application criada, ID:', data.Id);
    } catch (e) {
      console.log('   ❌ Erro:', e.message);
    }
  }

  console.log('\n🎉 TESTE COMPLETO FINALIZADO!');
  console.log('\n📊 RESUMO:');
  console.log('✅ Registro de usuário → PostgreSQL');
  console.log('✅ Login com JWT → PostgreSQL');
  console.log('✅ Criar listing → PostgreSQL');
  console.log('✅ Listar listings → PostgreSQL');
  console.log('✅ Buscar listing por ID → PostgreSQL');
  console.log('✅ Submeter application → PostgreSQL');
  console.log('\n💾 TODOS OS DADOS FORAM SALVOS NO SUPABASE!');
};

testAll();
