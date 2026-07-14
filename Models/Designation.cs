using Employee.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.API.Models
{

    [Table("designationTbl")]
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }

        [Required,MaxLength(50)]
        public string DesignationName { get; set; } = string.Empty;
    }


}




//USE [employeeManageDb]
//GO

///****** Object:  Table [dbo].[designationTbl]    Script Date: 6/25/2026 2:46:28 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[designationTbl](
//	[designationId] [int] IDENTITY(1,1) NOT NULL,
//	[departmentId] [int] NOT NULL,
//	[designationName] [varchar](50) NOT NULL,
// CONSTRAINT [PK_designationTbl] PRIMARY KEY CLUSTERED 
//(
//	[designationId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GO

//ALTER TABLE [dbo].[designationTbl]  WITH CHECK ADD  CONSTRAINT [FK_designationTbl_departmentTbl] FOREIGN KEY([departmentId])
//REFERENCES [dbo].[departmentTbl] ([departmentId])
//GO

//ALTER TABLE [dbo].[designationTbl] CHECK CONSTRAINT [FK_designationTbl_departmentTbl]
//GOUSE [employeeManageDb]
//GO

///****** Object:  Table [dbo].[designationTbl]    Script Date: 6/25/2026 2:46:28 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[designationTbl](
//	[designationId] [int] IDENTITY(1,1) NOT NULL,
//	[departmentId] [int] NOT NULL,
//	[designationName] [varchar](50) NOT NULL,
// CONSTRAINT [PK_designationTbl] PRIMARY KEY CLUSTERED 
//(
//	[designationId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GO

//ALTER TABLE [dbo].[designationTbl]  WITH CHECK ADD  CONSTRAINT [FK_designationTbl_departmentTbl] FOREIGN KEY([departmentId])
//REFERENCES [dbo].[departmentTbl] ([departmentId])
//GO

//ALTER TABLE [dbo].[designationTbl] CHECK CONSTRAINT [FK_designationTbl_departmentTbl]
//GO