﻿namespace Store.G04.core.Entities.OdrerEntities
{
    public class ProductItemOrder
    {

        public ProductItemOrder()
        {
            
        }

        public ProductItemOrder(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}