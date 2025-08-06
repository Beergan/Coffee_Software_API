# Coffee Shop Management System

Ứng dụng quản lý quán cà phê được xây dựng bằng ASP.NET Core Blazor Server và MudBlazor UI framework.

## 🌟 Tính năng

### Giao diện Nhân viên
- **Đăng nhập bảo mật** với ASP.NET Core Identity
- **Quản lý đơn hàng**: Tạo, cập nhật trạng thái đơn hàng
- **Xem sản phẩm**: Danh sách sản phẩm có sẵn với thông tin chi tiết

### Giao diện Quản lý (Manager/Admin)
- **Quản lý sản phẩm**: Thêm, sửa, xóa sản phẩm
- **Quản lý kho**: Cập nhật số lượng tồn kho
- **Báo cáo doanh thu**: Thống kê đơn hàng và doanh thu theo ngày
- **Quản lý nhân viên**: Phân quyền và quản lý tài khoản

## 🏗️ Kiến trúc

```
CoffeeShopSolution/
├── src/
│   ├── CoffeeShop.Server/     # Blazor Server App
│   │   ├── Components/        # Razor Components
│   │   ├── Data/             # ApplicationDbContext, SeedData
│   │   ├── Services/         # Business Logic Services
│   │   └── Program.cs        # App Configuration
│   │
│   └── CoffeeShop.Shared/    # Models & DTOs
│       ├── Models/           # Entity Models
│       └── DTOs/            # Data Transfer Objects
└── tests/                   # Unit & Integration Tests
```

## 📋 Models Chính

### User (Identity User)
- FirstName, LastName, Role (Staff/Manager/Admin)
- CreatedAt, LastLoginAt
- Navigation: Orders

### Product
- Name, Description, Price, Category
- IsAvailable, StockQuantity
- ImageUrl, CreatedAt, UpdatedAt

### Order
- OrderNumber, UserId, CustomerName
- TotalAmount, Status, CreatedAt, CompletedAt
- Navigation: User, OrderItems

### OrderItem
- OrderId, ProductId, Quantity, UnitPrice
- TotalPrice (computed), Notes

## 🛠️ Technologies

- **Frontend**: Blazor Server, MudBlazor (Material Design)
- **Backend**: ASP.NET Core 8.0
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **UI Framework**: MudBlazor 8.11.0

## 🚀 Chạy ứng dụng

### Yêu cầu
- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code

### Cài đặt

1. Clone repository
```bash
git clone <repository-url>
cd CoffeeShopSolution
```

2. Restore packages
```bash
dotnet restore
```

3. Chạy ứng dụng
```bash
cd src/CoffeeShop.Server
dotnet run
```

4. Mở trình duyệt và truy cập: `https://localhost:5001`

### Tài khoản mặc định

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@coffeeshop.com | Admin123! |
| Manager | manager@coffeeshop.com | Manager123! |
| Staff | staff@coffeeshop.com | Staff123! |

## 📊 Database Schema

Database sẽ được tự động tạo khi chạy ứng dụng lần đầu với:
- **Users**: Tài khoản người dùng và phân quyền
- **Products**: 15 sản phẩm mẫu (Coffee, Tea, Pastry, Sandwich, Snack)
- **Orders & OrderItems**: Cấu trúc đơn hàng

## 📱 Screenshots

### Dashboard
- Thống kê nhanh: Đơn hàng hôm nay, Doanh thu, Đơn đang chờ
- Quick Actions: Tạo đơn mới, Quản lý sản phẩm, Xem báo cáo
- Bảng đơn hàng gần đây

### Quản lý Sản phẩm
- Grid layout hiển thị sản phẩm với hình ảnh
- Tìm kiếm và lọc theo danh mục
- Dialog thêm/sửa sản phẩm
- Toggle trạng thái available/unavailable

### Quản lý Đơn hàng
- Bảng hiển thị tất cả đơn hàng
- Cập nhật trạng thái: Pending → Preparing → Ready → Completed
- Tìm kiếm theo số đơn, tên khách hàng
- Lọc theo trạng thái đơn hàng

## 🔐 Phân quyền

### Staff
- Xem dashboard
- Quản lý đơn hàng (tạo mới, cập nhật trạng thái)
- Xem danh sách sản phẩm

### Manager
- Tất cả quyền của Staff
- Quản lý sản phẩm (CRUD)
- Xem báo cáo doanh thu
- Quản lý kho hàng

### Admin
- Tất cả quyền của Manager
- Quản lý nhân viên
- Cài đặt hệ thống

## 🎨 UI Features

- **Responsive Design**: Hoạt động tốt trên desktop, tablet, mobile
- **Material Design**: Sử dụng MudBlazor components
- **Dark/Light Theme**: Hỗ trợ chuyển đổi theme
- **Real-time Updates**: Cập nhật trạng thái đơn hàng real-time
- **Loading States**: Hiển thị loading cho các action async
- **Error Handling**: Thông báo lỗi và success messages

## 📈 Tương lai

### Các tính năng có thể mở rộng:
- **Real-time Notifications**: SignalR cho thông báo đơn hàng mới
- **Inventory Management**: Quản lý nhập kho, xuất kho chi tiết
- **Customer Management**: Quản lý khách hàng thân thiết
- **Reporting**: Báo cáo chi tiết theo tuần/tháng/năm
- **Mobile App**: Ứng dụng mobile cho nhân viên
- **POS Integration**: Tích hợp với máy POS
- **Loyalty Program**: Chương trình tích điểm khách hàng

## 💡 Contribution

Mọi đóng góp đều được chào đón! Vui lòng tạo issue hoặc pull request.

## 📄 License

Dự án này được phát hành dưới MIT License.