/* Options:
Date: 2014-12-10 00:26:05
Version: 1
BaseUrl: http://localhost:32494

GlobalNamespace: dtos
//MakePropertiesOptional: True
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion: 
*/

declare module dtos
{

    interface IReturnVoid
    {
    }

    interface IReturn<T>
    {
    }

    interface Question
    {
        QuestionId?:number;
        Tags?:string[];
        Owner?:User;
        IsAnswered?:boolean;
        ViewCount?:number;
        AnswerCount?:number;
        Score?:number;
        LastActivityDate?:number;
        CreationDate?:number;
        LastEditDate?:number;
        Link?:string;
        Title?:string;
        AcceptedAnswerId?:number;
    }

    interface Answer
    {
        AnswerId?:number;
        Owner?:User;
        IsAccepted?:boolean;
        Score?:number;
        LastActivityDate?:number;
        LastEditDate?:number;
        CreationDate?:number;
        QuestionId?:number;
    }

    // @DataContract
    interface RestService
    {
        // @DataMember(Name="path")
        Path?:string;

        // @DataMember(Name="description")
        Description?:string;
    }

    interface QueryBase_1<T> extends QueryBase
    {
    }

    // @DataContract
    interface ResponseStatus
    {
        // @DataMember(Order=1)
        ErrorCode?:string;

        // @DataMember(Order=2)
        Message?:string;

        // @DataMember(Order=3)
        StackTrace?:string;

        // @DataMember(Order=4)
        Errors?:ResponseError[];
    }

    interface User
    {
        Reputation?:number;
        Userid?:number;
        UserType?:string;
        AcceptRate?:number;
        ProfileImage?:string;
        DisplayName?:string;
        Link?:string;
    }

    interface QueryBase
    {
        // @DataMember(Order=1)
        Skip?:number;

        // @DataMember(Order=2)
        Take?:number;

        // @DataMember(Order=3)
        OrderBy?:string;

        // @DataMember(Order=4)
        OrderByDesc?:string;
    }

    // @DataContract
    interface ResponseError
    {
        // @DataMember(Order=1, EmitDefaultValue=false)
        ErrorCode?:string;

        // @DataMember(Order=2, EmitDefaultValue=false)
        FieldName?:string;

        // @DataMember(Order=3, EmitDefaultValue=false)
        Message?:string;
    }

    interface SearchQuestionsResponse
    {
        Results?:Question[];
    }

    interface GetAnswersResponse
    {
        Ansnwer?:Answer;
    }

    interface GetStatsResponse
    {
        QuestionsCount?:number;
        AnswersCount?:number;
        TagCounts?:{ [index:string]: number; };
        TopQuestionScore?:number;
        TopQuestionViews?:number;
        TopAnswerScore?:number;
    }

    // @DataContract
    interface ResourcesResponse
    {
        // @DataMember(Name="swaggerVersion")
        SwaggerVersion?:string;

        // @DataMember(Name="apiVersion")
        ApiVersion?:string;

        // @DataMember(Name="basePath")
        BasePath?:string;

        // @DataMember(Name="apis")
        Apis?:RestService[];
    }

    // @DataContract
    interface QueryResponse<Question>
    {
        // @DataMember(Order=1)
        Offset?:number;

        // @DataMember(Order=2)
        Total?:number;

        // @DataMember(Order=3)
        Results?:Question[];

        // @DataMember(Order=4)
        Meta?:{ [index:string]: string; };

        // @DataMember(Order=5)
        ResponseStatus?:ResponseStatus;
    }

    // @Route("/questions/search")
    interface SearchQuestions extends IReturn<SearchQuestionsResponse>
    {
        Tags?:string[];
        UserId?:string;
    }

    /**
    * Get a list of Answers for a Question
    */
    // @Route("/answers/{QuestionId}")
    interface GetAnswers extends IReturn<GetAnswersResponse>
    {
        QuestionId?:number;
    }

    // @Route("/admin/stats", "GET")
    interface GetStats extends IReturn<GetStats>
    {
    }

    // @Route("/resources")
    // @DataContract
    interface Resources extends IReturn<Resources>
    {
        // @DataMember(Name="apiKey")
        ApiKey?:string;
    }

    // @Route("/resource/{Name*}")
    // @DataContract
    interface ResourceRequest
    {
        // @DataMember(Name="apiKey")
        ApiKey?:string;

        // @DataMember(Name="name")
        Name?:string;
    }

    // @Route("/postman")
    interface Postman
    {
        Label?:string[];
        ExportSession?:boolean;
        ssid?:string;
        sspid?:string;
        ssopt?:string;
    }

    // @Route("/questions")
    interface StackOverflowQuery extends QueryBase_1<Question>, IReturn<QueryResponse<Question>>
    {
        ScoreGreaterThan?:number;
    }

}
