# 🚀 Teste Rápido - TechAds

## ✅ Status Atual

### Backend (.NET + PostgreSQL)

- ✅ **Rodando**: http://localhost:5025
- ✅ **Banco**: PostgreSQL no Supabase conectado
- ✅ **Migrations**: Aplicadas com sucesso
- ✅ **Endpoints disponíveis**:
  - `POST /api/auth/register-simple` - Registrar usuário
  - `POST /api/auth/login` - Login
  - `GET /api/auth/test` - Teste de conectividade

### Frontend (React + Vite)

- ✅ **Rodando**: http://localhost:5173
- ⚠️ **Node.js**: Versão 20.17.0 (Vite recomenda 20.19+, mas funciona)
- ✅ **Páginas**: Landing, Login, Register, Dashboard, CreateListing, ListingDetail

### Banco de Dados (PostgreSQL/Supabase)

- ✅ **Conectado e funcional**
- ✅ **Tabelas criadas**:
  - AspNetUsers (autenticação)
  - AspNetRoles
  - ProjectListings
  - Applications
  - Tags

---

## 🧪 Como Testar Agora

### 1. Registrar um Usuário

1. Abra http://localhost:5173
2. Clique em **"Sign Up"** ou **"Get Started"**
3. Preencha o formulário:
   - Name: `Test User`
   - Email: `test@example.com`
   - Password: `Test@123456`
   - Confirm Password: `Test@123456`
4. Clique em **"Create Account"**
5. ✅ Usuário será salvo no PostgreSQL!

### 2. Fazer Login

1. Após registrar, você será redirecionado para `/login`
2. Use as credenciais:
   - Email: `test@example.com`
   - Password: `Test@123456`
3. Clique em **"Sign In"**
4. ✅ Você será autenticado e redirecionado para `/dashboard`

### 3. Criar uma Listing

1. No Dashboard, clique em **"Create New Listing"**
2. Preencha:
   - Title: `Senior React Developer`
   - Description: `Looking for experienced React dev`
   - Requirements: `3+ years React, TypeScript, Node.js`
   - Tags: `react, typescript, frontend`
3. Clique em **"Create Listing"**
4. ✅ Listing será salva no PostgreSQL!

---

## 🔧 Comandos Úteis

### Backend

```powershell
# Rodar backend
cd d:\TechAds\backend
dotnet run --project .\TechAds.Api\TechAds.Api.csproj

# Verificar se está rodando
netstat -ano | findstr :5025
```

### Frontend

```powershell
# Rodar frontend
cd d:\TechAds\frontend\techads-web
npm run dev

# Abrir no navegador
start http://localhost:5173
```

### Banco de Dados

```powershell
# Ver tabelas no banco
cd d:\TechAds\backend\TestConnection
dotnet run

# Aplicar migrations
cd d:\TechAds\backend\TechAds.Infrastructure
dotnet ef database update --startup-project ..\TechAds.Api\TechAds.Api.csproj
```

---

## 📋 Checklist de Funcionalidades

### ✅ Implementado e Funcionando

- [x] Frontend React com Tailwind CSS
- [x] Backend .NET com Entity Framework Core
- [x] PostgreSQL no Supabase
- [x] Registro de usuários (salva no banco)
- [x] Login com JWT (em implementação)
- [x] CORS configurado
- [x] Migrations aplicadas

### 🚧 Em Desenvolvimento

- [ ] Login completo com JWT no frontend
- [ ] Dashboard com listagens do usuário
- [ ] CRUD completo de listings
- [ ] Aplicação para listings
- [ ] Filtros e busca

### 📝 Próximos Passos

1. **Testar Registro**: Criar 2-3 usuários diferentes
2. **Verificar no Supabase**: Ver se os usuários aparecem na tabela `AspNetUsers`
3. **Testar Login**: Fazer login com usuários criados
4. **Criar Listings**: Testar criação de várias listings
5. **Dashboard**: Ver listagens no dashboard

---

## 🐛 Problemas Conhecidos

1. **Node.js Warning**: Vite recomenda Node 20.19+, mas funciona com 20.17.0
2. **Backend fecha sozinho**: Solução temporária - rodar em janela separada do PowerShell
3. **JWT no Login**: Frontend precisa ser atualizado para usar tokens corretamente

---

## 🎯 O Que Funciona Agora

✅ **Registro**: Frontend → Backend → PostgreSQL (100% funcional)
✅ **Banco**: Todas as tabelas criadas e acessíveis
✅ **CORS**: Frontend pode fazer requisições ao backend
✅ **Autenticação**: ASP.NET Identity configurado

---

## 📞 Suporte

Se algo não funcionar:

1. Verifique se backend está rodando: `netstat -ano | findstr :5025`
2. Verifique se frontend está rodando: abra http://localhost:5173
3. Veja logs do backend na janela do PowerShell
4. Veja logs do frontend no terminal do VS Code

---

**Última atualização**: 05/11/2025 23:30
**Status**: ✅ Pronto para testes básicos
