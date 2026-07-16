using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public class ProductInventoryTransactionDto
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// شناسه محصول
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// شناسه واریانت محصول (در صورت وجود)
        /// </summary>
        public Guid? ProductVariantId { get; set; }

        /// <summary>
        /// نوع عملیات موجودی - 1 افزایش، 2 کاهش
        /// </summary>
        public byte OperationType { get; set; }

        /// <summary>
        /// تعداد تغییر یافته موجودی
        /// </summary>
        public int ChangeQuantity { get; set; }

        /// <summary>
        /// شناسه سند اصلی مرتبط مانند OrderId
        /// </summary>
        public Guid? ReferenceId { get; set; }

        /// <summary>
        /// شناسه جزئیات سند مرتبط مانند OrderItemId
        /// </summary>
        public Guid? ReferenceDetailId { get; set; }

        /// <summary>
        /// توضیحات تکمیلی تراکنش
        /// </summary>
        [StringLength(500)]
        public string? Note { get; set; }

        /// <summary>
        /// زمان انجام تراکنش موجودی
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// کاربر ایجاد کننده تراکنش
        /// </summary>
        public Guid? CreatedBy { get; set; }

        public Guid ReasonId { get; set; }
    }
}
