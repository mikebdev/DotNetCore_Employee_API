using Employee.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.API.Models
{

    [Table("departmentTbl")]
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required, MaxLength(50)]
        public string DepartmentName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

    }
}




//USE [employeeManageDb]
//GO

///****** Object:  Table [dbo].[departmentTbl]    Script Date: 6/25/2026 2:39:34 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[departmentTbl](
//	[departmentId] [int] IDENTITY(1,1) NOT NULL,
//	[departmentName] [varchar](50) NOT NULL,
//	[isActive] [bit] NULL,
// CONSTRAINT [PK_departmentTbl] PRIMARY KEY CLUSTERED 
//(
//	[departmentId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GOUSE [employeeManageDb]
//GO

///****** Object:  Table [dbo].[departmentTbl]    Script Date: 6/25/2026 2:39:34 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[departmentTbl](
//	[departmentId] [int] IDENTITY(1,1) NOT NULL,
//	[departmentName] [varchar](50) NOT NULL,
//	[isActive] [bit] NULL,
// CONSTRAINT [PK_departmentTbl] PRIMARY KEY CLUSTERED 
//(
//	[departmentId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GO
