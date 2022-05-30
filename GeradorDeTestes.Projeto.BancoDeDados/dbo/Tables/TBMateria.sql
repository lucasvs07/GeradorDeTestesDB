CREATE TABLE [dbo].[TBMateria] (
    [Numero]            INT           NOT NULL,
    [Nome]              VARCHAR (150) NOT NULL,
    [Serie]             INT           NOT NULL,
    [Disciplina_Numero] INT           NOT NULL,
    [Disciplina_Nome]   VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_TBMateria] PRIMARY KEY CLUSTERED ([Numero] ASC),
    CONSTRAINT [FK_TBMateria_TBMateria] FOREIGN KEY ([Disciplina_Numero]) REFERENCES [dbo].[TBMateria] ([Numero])
);

