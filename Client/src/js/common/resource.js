export const CommandName = {
  NoAction: "", // rỗng
  Add: "add", // thêm
  Edit: "edit", // sửa
  Delete: "delete", // xóa
  Notice: "notice", // thông báo
  Duplicate: "duplicate", // nhân bản
  Import: "import", // nhập khẩu
};

export const CommandNameFormDetail = {
  NoAction: "", // rỗng
  Save: "save", // thêm
  Close: "close", // sửa
  Notice: "notice", // xóa
};

export const CommandNameFormConfirm = {
  NoAction: "", // rỗng
  Add: "add", // thêm
  Edit: "edit", // sửa
  Delete: "delete", // xóa
  Notice: "notice", // thông báo
  Duplicate: "duplicate", // nhân bản
  Import: "import", // nhập khẩu
  Save: "save", // Lưu
  Close: "close", // đóng
};

export const Methods = {
  Post: "post", // thêm
  Get: "get", // lấy thông tin
  Put: "put", //  sửa
  Delete: "delete", // xóa
};

export const NoticeName = {
  Increase: "increase",
  Increase1: "increase1",
  NoAction: "",
};

export const GuidEmpty = "00000000-0000-0000-0000-000000000000";

export const ErrorMsg = {
  Duplicate: "đã tồn tại!",
  AttritionValue: "phải nhỏ hơn nguyên giá!",
  AttritionRate: "Tỷ lệ hao mòn phải bằng 1/số năm sử dụng.",
  UsedYear: "Số năm sử dụng phải bằng 1/tỷ lệ hao mòn.",
  Format: "sai định dạng ('TS' + ít nhất 5 chữ số).",
  Required: "không được để trống!",
  ImportErr:
    "Chưa có bản ghi nào được nhập khẩu thành công! Vui lòng kiểm tra lại dữ liệu(thiếu dữ liệu, mã tài sản đã tồn tại...).",
  TypeFileErr: "Vui lòng chọn đúng định dạng file excel!",
  NotChooseProperty:
    "Chưa có tài sản nào được chọn. Vui lòng chọn trước khi thực hiện xóa!",
  AttritionValueLessThanMarkedPrice: "Tỷ lệ hao mòn phải nhỏ hơn nguyên giá",
  Min: "phải có giá trị lớn hơn",
  Max: "phải có giá trị nhỏ hơn",
  MinLength: "phải lớn hơn",
  MaxLength: "phải nhỏ hơn",
  Prefix: "Phải bắt đầu bằng tiền tố",
  ListPropertyNotEmpty: "Chứng từ ghi tăng phải có ít nhất 1 tài sản!",
  NotChooseIncrementProperty:
    "Chưa có chứng từ nào được chọn. Vui lòng chọn chứng từ trước khi xóa!",
  Empty: "",
};

export const NoticeMsg = {
  PropertyEdited:
    "Thông tin tài sản đã được thay đổi. Bạn có muốn lưu thông tin thay đôi này?",
  IncrementPropertyEdited:
    "Thông tin tài sản đã được thay đổi. Bạn có muốn lưu thông tin thay đôi này?",
  DataRequired: "Vui lòng nhập đủ dữ liệu trước khi chọn lưu!",
  DuplicateBudget: "Nguồn hình thành không được phép trùng nhau!",
};

export const TitlePopup = {
  EditProperty: "Sửa tài sản",
  AddProperty: "Thêm tài sản",
  AddPropertyType: "Thêm loại tài sản",
  EditPropertyType: "Sửa loại tài sản",
};

export const TypeOfExcel =
  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
