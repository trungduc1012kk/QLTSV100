// Các mode của form detail
export const FormDetailMode = {
  Add: 1, // Thêm mới
  Edit: 2, // Sửa
};

// Các mode của form confirm
export const FormConfirmMode = {
  Success: 1, // Thành công
  Warning: 2, // Cảnh báo
  Error: 3, // Lỗi
  Info: 4, // Thông báo
};

// Các mode của form notice
export const FormNoticeMode = {
  Success: 1, // Thành công
  Warning: 2, // Cảnh báo
  Error: 3, // Lỗi
  Info: 4, // Thông báo
};

export const ErrorCode = {
  Exception: 1, // lỗi chung
  Validate: 2, // lỗi validate
  NotFound: 3, // lỗi không tìm thấy dữ liệu
  ServerError: 4, // lỗi server
  DuplicateCode: 5, // lỗi trùng mã
};

export const PropertyStatus = {
  Normal: 0,
  Increased: 1,
};
