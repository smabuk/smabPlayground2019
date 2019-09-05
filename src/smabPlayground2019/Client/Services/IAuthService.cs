using System.Threading.Tasks;
using smabPlayground2019.Shared;

namespace smabPlayground2019.Client.Services
{
	public interface IAuthService
	{
		Task<LoginResult> Login(LoginModel loginModel);
		Task Logout();
		Task<RegisterResult> Register(RegisterModel registerModel);
	}
}
