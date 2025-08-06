# 🔄 Hướng dẫn Update Migrations - Coffee Shop App

## 🚀 **Cách 1: Auto Migration (Khuyến nghị - Đã cấu hình sẵn)**

Ứng dụng **tự động update database** khi khởi động:

```bash
cd src/CoffeeShop.Server
dotnet run
```

✅ **Ứng dụng sẽ tự động:**
- Kiểm tra pending migrations
- Áp dụng migrations mới
- Tạo database nếu chưa có
- Seed dữ liệu mẫu

## 🛠️ **Cách 2: Sử dụng Admin UI**

### Bước 1: Chạy ứng dụng
```bash
cd src/CoffeeShop.Server
dotnet run
```

### Bước 2: Đăng nhập Admin
- **URL:** `http://localhost:5000`
- **Email:** `admin@coffeeshop.com`  
- **Password:** `Admin123!`

### Bước 3: Database Management
**Admin → Database Management**
- ✅ **Check Pending Migrations** - Xem migrations chưa apply
- ✅ **Apply Migrations** - Áp dụng migrations mới
- ✅ **Force Recreate Database** - Reset hoàn toàn database
- ✅ **Reseed Data** - Tạo lại dữ liệu mẫu

## ⚡ **Cách 3: Sử dụng dotnet ef CLI**

### Cài đặt EF Tools (nếu chưa có):
```bash
dotnet tool install --global dotnet-ef
```

### Lệnh migrations cơ bản:

#### Tạo migration mới:
```bash
cd src/CoffeeShop.Server
dotnet ef migrations add <TenMigration>

# Ví dụ:
dotnet ef migrations add AddProductCategory
dotnet ef migrations add UpdateOrderTable
```

#### Áp dụng migrations:
```bash
# Áp dụng tất cả pending migrations
dotnet ef database update

# Áp dụng đến migration cụ thể
dotnet ef database update <TenMigration>

# Ví dụ:
dotnet ef database update AddProductCategory
```

#### Xem danh sách migrations:
```bash
# Xem tất cả migrations
dotnet ef migrations list

# Xem pending migrations
dotnet ef migrations list --pending
```

#### Tạo script SQL:
```bash
# Tạo script cho tất cả migrations
dotnet ef migrations script

# Tạo script từ migration này đến migration kia
dotnet ef migrations script <FromMigration> <ToMigration>

# Tạo script cho production
dotnet ef migrations script --output migrations.sql
```

#### Xóa migration:
```bash
# Xóa migration cuối cùng (chưa apply)
dotnet ef migrations remove

# Force xóa (nguy hiểm)
dotnet ef migrations remove --force
```

#### Reset database:
```bash
# Xóa database hoàn toàn
dotnet ef database drop

# Tạo lại từ đầu
dotnet ef database update
```

## 🗄️ **Migrations cho SQL Server vs SQLite**

### Current Configuration:
- **SQL Server:** `NGOMANHCUONG\SQLEXPRESS` → Database: `RuomRaCoffe`
- **SQLite:** `CoffeeShop.db` (fallback)

### Automatic Provider Selection:
Ứng dụng **tự động chọn** database provider dựa trên connection string:

```csharp
// SQL Server: chứa "Initial Catalog", "Database=", hoặc "SQLEXPRESS"
// SQLite: tất cả các trường hợp khác
```

## 📋 **Current Database Schema**

Sau khi migrations, database sẽ có:

### Identity Tables:
- `AspNetUsers` - User accounts
- `AspNetRoles` - User roles (Admin, Manager, Staff)  
- `AspNetUserRoles` - User-role relationships

### Business Tables:
- `Products` - Coffee shop products
- `Orders` - Customer orders
- `OrderItems` - Order line items

### Sample Data:
- **3 roles:** Admin, Manager, Staff
- **3 users:** admin@coffeeshop.com, manager@coffeeshop.com, staff@coffeeshop.com
- **15 products:** Various coffee shop items

## 🚨 **Troubleshooting**

### Lỗi: "The provider for the source is different"
```bash
# Xóa migrations folder và tạo lại
rm -rf Migrations/
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Lỗi: "Cannot access a disposed object"  
```bash
# Restart ứng dụng
# Hoặc dùng Admin UI để force recreate
```

### Lỗi: Connection string issues
```bash
# Kiểm tra appsettings.json
# Đảm bảo SQL Server đang chạy
# Hoặc chuyển về SQLite tạm thời
```

### Migration bị conflict:
```bash
# Xóa migration lỗi
dotnet ef migrations remove

# Tạo lại
dotnet ef migrations add <TenMoi>
```

## 🎯 **Best Practices**

### 1. **Backup trước khi migrate:**
```sql
-- SQL Server
BACKUP DATABASE RuomRaCoffe TO DISK = 'C:\backup\RuomRaCoffe.bak'
```

### 2. **Test migration trên development trước:**
```bash
# Development environment
dotnet ef database update --environment Development
```

### 3. **Sử dụng descriptive migration names:**
```bash
# ❌ Tránh
dotnet ef migrations add Update1

# ✅ Nên dùng  
dotnet ef migrations add AddProductCategoryAndPrice
dotnet ef migrations add FixOrderItemsRelationship
```

### 4. **Review migration trước khi apply:**
```bash
# Xem script sẽ được thực thi
dotnet ef migrations script --output review.sql
```

## 🔄 **Common Scenarios**

### Thêm column mới:
1. Sửa model class
2. `dotnet ef migrations add AddNewColumn`
3. `dotnet ef database update`

### Thay đổi relationship:
1. Sửa navigation properties  
2. `dotnet ef migrations add UpdateRelationships`
3. Review generated script
4. `dotnet ef database update`

### Rename table/column:
1. Sử dụng `RenameTable()`/`RenameColumn()` trong migration
2. Test thoroughly trước khi production

---

## ✅ **Quick Commands Summary**

```bash
# 🚀 Nhanh nhất - Auto migration
dotnet run

# 🔍 Kiểm tra migrations
dotnet ef migrations list

# ➕ Tạo migration mới  
dotnet ef migrations add <Name>

# 🔄 Áp dụng migrations
dotnet ef database update

# 📜 Tạo SQL script
dotnet ef migrations script

# 🗑️ Xóa migration cuối
dotnet ef migrations remove

# 💥 Reset database
dotnet ef database drop && dotnet ef database update
```

**Khuyến nghị: Sử dụng Auto Migration hoặc Admin UI cho đơn giản!** 🎉