# 🔧 Hướng dẫn Setup SQL Server cho Database có đầy đủ bảng

## ✅ **Trạng thái hiện tại: Database hoạt động với SQLite**

Database đã được tạo thành công với tất cả bảng cần thiết. Để chuyển sang SQL Server:

## 🔄 **Cách chuyển từ SQLite sang SQL Server**

### **Bước 1: Đảm bảo SQL Server đang chạy**
```cmd
# Kiểm tra SQL Server Service
services.msc

# Hoặc start SQL Server Express
net start MSSQL$SQLEXPRESS

# Verify SQL Server accessible
sqlcmd -S NGOMANHCUONG\SQLEXPRESS -U sa -P JJQQKKAA
```

### **Bước 2: Tạo database RuomRaCoffe**
```sql
-- Connect to SQL Server Management Studio (SSMS)
-- Server: NGOMANHCUONG\SQLEXPRESS
-- User: sa, Password: JJQQKKAA

-- Create database
CREATE DATABASE RuomRaCoffe;
```

### **Bước 3: Chuyển connection string sang SQL Server**

Sửa file `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=NGOMANHCUONG\\SQLEXPRESS;Initial Catalog=RuomRaCoffe;User ID=sa;Password=JJQQKKAA;TrustServerCertificate=True;",
    "SqliteConnection": "Data Source=CoffeeShop.db"
  }
}
```

### **Bước 4: Chạy ứng dụng**
```bash
cd src/CoffeeShop.Server
dotnet run
```

## 🗄️ **Database Schema sẽ được tạo:**

### **Identity Tables:**
- `AspNetUsers` - User accounts với custom fields
- `AspNetRoles` - Roles (Admin, Manager, Staff)
- `AspNetUserRoles` - User-role relationships
- `AspNetUserClaims`, `AspNetRoleClaims` - Claims
- `AspNetUserLogins`, `AspNetUserTokens` - External logins, tokens

### **Business Tables:**
- `Products` - Coffee shop inventory
  - `Id`, `Name`, `Description`, `Price`, `Category`, `IsAvailable`, `StockQuantity`
- `Orders` - Customer orders
  - `Id`, `OrderNumber`, `UserId`, `CustomerName`, `TotalAmount`, `Status`, `CreatedAt`
- `OrderItems` - Order line items
  - `Id`, `OrderId`, `ProductId`, `Quantity`, `UnitPrice`, `Notes`

### **Sample Data:**
- **3 Users:**
  - `admin@coffeeshop.com / Admin123!`
  - `manager@coffeeshop.com / Manager123!`
  - `staff@coffeeshop.com / Staff123!`
- **15 Products:** Coffee, Tea, Pastry items
- **3 Roles:** Admin, Manager, Staff

## 📋 **Commands để update database:**

### **Auto Migration (Khuyến nghị):**
```bash
dotnet run
# Ứng dụng tự động: Check → Create/Update database → Seed data
```

### **Admin UI:**
1. Đăng nhập: `admin@coffeeshop.com / Admin123!`
2. **Admin → Database Management**
3. **Force Recreate Database** (để chuyển từ SQLite)

### **Manual Migration:**
```bash
# Nếu cần reset hoàn toàn
dotnet ef database drop --force
dotnet ef database update
```

## 🚨 **Troubleshooting SQL Server Connection:**

### **Lỗi: "Server was not found or was not accessible"**
**Giải pháp:**
1. **Kiểm tra SQL Server Configuration Manager:**
   - Enable TCP/IP protocol
   - Start SQL Server Browser service
   - Check dynamic port or set static port

2. **Enable SQL Server Authentication:**
   ```sql
   -- In SSMS, right-click server → Properties → Security
   -- Select "SQL Server and Windows Authentication mode"
   ```

3. **Unlock sa account:**
   ```sql
   ALTER LOGIN sa ENABLE;
   ALTER LOGIN sa WITH PASSWORD = 'JJQQKKAA';
   ```

### **Lỗi: "Login failed for user 'sa'"**
**Giải pháp:**
1. Reset sa password
2. Enable mixed mode authentication
3. Check SQL Server services running

### **Network issues:**
1. Check Windows Firewall
2. Enable SQL Server Browser
3. Check port 1433 or dynamic port
4. Try with IP address instead of server name

## 🔄 **Fallback to SQLite:**

Nếu SQL Server không hoạt động, luôn có thể quay lại SQLite:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CoffeeShop.db"
  }
}
```

## ✅ **Verification:**

Sau khi setup thành công, verify:
1. **Database tables:** Kiểm tra tất cả bảng đã được tạo
2. **Sample data:** Verify users, roles, products exists
3. **Login test:** Test đăng nhập với admin account
4. **Admin UI:** Access Database Management page

## 🎯 **Production Notes:**

1. **Security:**
   - Đổi sa password
   - Tạo dedicated database user thay vì sa
   - Enable encryption if needed

2. **Performance:**
   - Add additional indexes if needed
   - Configure backup strategy
   - Monitor query performance

3. **Deployment:**
   - Use appsettings.Production.json for production
   - Store connection strings securely
   - Consider using Azure SQL Database or SQL Managed Instance

---

**Database hiện tại đã có đầy đủ bảng và hoạt động tốt. Việc chuyển sang SQL Server chỉ cần thay connection string!** 🎉