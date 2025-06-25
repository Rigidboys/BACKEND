namespace RigidboysAPI.Errors
{
    public static class ErrorCodes
    {
        public const string DUPLICATE_CUSTOMER = "DUPLICATE_CUSTOMER";
        public const string DUPLICATE_CUSTOMER_MESSAGE = "이미 등록된 고객사입니다.";

        public const string DUPLICATE_PRODUCT = "DUPLICATE_PRODUCT";
        public const string DUPLICATE_PRODUCT_MESSAGE = "이미 등록된 제품입니다.";

        public const string DUPLICATE_PURCHASE = "DUPLICATE_PURCHASE";
        public const string DUPLICATE_PURCHASE_MESSAGE= "이미 등록된 매입 / 매출 정보입니다.";

        public const string INVALID_INPUT = "INVALID_INPUT";
        public const string INVALID_INPUT_MESSAGE = "입력값이 유효하지 않습니다.";

        public const string SERVER_ERROR = "SERVER_ERROR";
        public const string SERVER_ERROR_MESSAGE = "서버 내부 오류가 발생했습니다.";

        public const string CUSTOMER_NOT_FOUND = "CUSTOMER_NOT_FOUND";
        public const string CUSTOMER_NOT_FOUND_MESSAGE = "해당 고객을 찾을 수 없습니다.";


        public const string PRODUCT_NOT_FOUND = "PRODUCT_NOT_FOUND";
        public const string PRODUCT_NOT_FOUND_MESSAGE = "해당 제품을 찾을 수 없습니다.";

        public const string PURCHASE_NOT_FOUND = "PURCHASE_NOT_FOUND";
        public const string PURCHASE_NOT_FOUND_MESSAGE = "해당 매입 / 매출을 찾을 수 없습니다.";

    }
}
