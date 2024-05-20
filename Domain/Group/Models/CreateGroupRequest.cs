namespace Domain.Group.Models;

public class CreateGroupRequest
{
    
        public CreateGroupRequest(string name)
        {
            Name = name;
        }

        public CreateGroupRequest() { }
        public string Name { get; set; }
   
}