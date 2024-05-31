namespace Presentation.Model.Requests;

public class CreateGroupRequest
{
        public CreateGroupRequest(string name)
        {
            Name = name;
        }

        public CreateGroupRequest() { }
        public string Name { get; set; }
   
}