using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.API.Models
{
    [Table("employeeTbl")]
    public class EmployeeL
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required,MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required,MaxLength(50)]
        public string Password { get; set; } = string.Empty; // password matching the login DTO

        [Required,MaxLength(15), MinLength(10)]
        public string Phone { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Role {  get; set; } = string.Empty;
    }



    //// just for testing, would be way more to add for a production app, JWT and so on.
    //public class LoginDto
    //{
    //    [Required, EmailAddress]
    //    public string email { get; set; } = string.Empty;

    //    [Required]
    //    public string phone { get; set; } = string.Empty;
    //}




}



//USE [employeeManageDb]
//GO

///****** Object:  Table [dbo].[employeeTbl]    Script Date: 6/25/2026 2:55:18 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[employeeTbl](
//	[employeeId] [int] IDENTITY(1,1) NOT NULL,
//	[name] [varchar](50) NOT NULL,
//	[phone] [varchar](15) NOT NULL,
//	[mobilePhone] [varchar](15) NULL,
//	[email] [varchar](50) NOT NULL,
//	[address] [varchar](250) NOT NULL,
//	[city] [varchar](50) NOT NULL,
//	[state] [varchar](50) NOT NULL,
//	[zipcode] [varchar](12) NOT NULL,
//	[designationId] [int] NOT NULL,
//	[createdDate] [datetime] NULL,
//	[modifiedDate] [datetime] NULL,
// CONSTRAINT [PK_employeeTbl] PRIMARY KEY CLUSTERED 
//(
//	[employeeId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GO

//ALTER TABLE [dbo].[employeeTbl]  WITH CHECK ADD  CONSTRAINT [FK_employeeTbl_designationTbl] FOREIGN KEY([designationId])
//REFERENCES [dbo].[designationTbl] ([designationId])
//GO

//ALTER TABLE [dbo].[employeeTbl] CHECK CONSTRAINT [FK_employeeTbl_designationTbl]
//GOUSE [employeeManageDb]
//GO

///****** Object:  Table [dbo].[employeeTbl]    Script Date: 6/25/2026 2:55:18 PM ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[employeeTbl](
//	[employeeId] [int] IDENTITY(1,1) NOT NULL,
//	[name] [varchar](50) NOT NULL,
//	[phone] [varchar](15) NOT NULL,
//	[mobilePhone] [varchar](15) NULL,
//	[email] [varchar](50) NOT NULL,
//	[address] [varchar](250) NOT NULL,
//	[city] [varchar](50) NOT NULL,
//	[state] [varchar](50) NOT NULL,
//	[zipcode] [varchar](12) NOT NULL,
//	[designationId] [int] NOT NULL,
//	[createdDate] [datetime] NULL,
//	[modifiedDate] [datetime] NULL,
// CONSTRAINT [PK_employeeTbl] PRIMARY KEY CLUSTERED 
//(
//	[employeeId] ASC
//)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
//) ON [PRIMARY]
//GO

//ALTER TABLE [dbo].[employeeTbl]  WITH CHECK ADD  CONSTRAINT [FK_employeeTbl_designationTbl] FOREIGN KEY([designationId])
//REFERENCES [dbo].[designationTbl] ([designationId])
//GO

//ALTER TABLE [dbo].[employeeTbl] CHECK CONSTRAINT [FK_employeeTbl_designationTbl]
//GO