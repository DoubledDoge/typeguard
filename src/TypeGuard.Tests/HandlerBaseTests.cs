using Imposter.Abstractions;
using TypeGuard.Core.Handlers;
using TypeGuard.Core.Interfaces;

[assembly: GenerateImposter(typeof(IInputProvider))]
[assembly: GenerateImposter(typeof(IOutputProvider))]
[assembly: GenerateImposter(typeof(IValidatorRule<>))]
[assembly: GenerateImposter(typeof(HandlerBase<>))]

namespace TypeGuard.Tests;

internal sealed class HandlerBaseTests
{
	private const string DefaultPrompt = "Enter a number:";

	[Test]
	public async Task Constructor_NullInputProvider_ThrowsArgumentNullException()
	{
		// Arrange
		IOutputProvider outputProvider = IOutputProvider.Imposter().Instance();

		// Act & Assert
		ArgumentNullException? exception = await Assert
			.That(() => HandlerBase<int>.Imposter(null!, outputProvider, DefaultPrompt).Instance())
			.ThrowsExactly<ArgumentNullException>();

		await Assert.That(exception!.ParamName).IsEqualTo("inputProvider");
	}

	[Test]
	public async Task Constructor_NullOutputProvider_ThrowsArgumentNullException()
	{
		// Arrange
		IInputProvider inputProvider = IInputProvider.Imposter().Instance();

		// Act & Assert
		ArgumentNullException? exception = await Assert
			.That(() => HandlerBase<int>.Imposter(inputProvider, null!, DefaultPrompt).Instance())
			.ThrowsExactly<ArgumentNullException>();

		await Assert.That(exception!.ParamName).IsEqualTo("outputProvider");
	}

	[Test]
	public async Task Constructor_NullPrompt_ThrowsArgumentNullException()
	{
		// Arrange
		IInputProvider inputProvider = IInputProvider.Imposter().Instance();
		IOutputProvider outputProvider = IOutputProvider.Imposter().Instance();

		// Act & Assert
		ArgumentNullException? exception = await Assert
			.That(() => HandlerBase<int>.Imposter(inputProvider, outputProvider, null!).Instance())
			.ThrowsExactly<ArgumentNullException>();

		await Assert.That(exception!.ParamName).IsEqualTo("prompt");
	}

	[Test]
	public async Task AddRule_NullRule_ThrowsArgumentNullException()
	{
		// Arrange
		IInputProvider inputProvider = IInputProvider.Imposter().Instance();
		IOutputProvider outputProvider = IOutputProvider.Imposter().Instance();
		HandlerBase<int> sut = HandlerBase<int>
			.Imposter(inputProvider, outputProvider, DefaultPrompt)
			.Instance();

		// Act & Assert
		ArgumentNullException? exception = await Assert
			.That(() => sut.AddRule(null!))
			.ThrowsExactly<ArgumentNullException>();

		await Assert.That(exception!.ParamName).IsEqualTo("rule");
	}

	[Test]
	public async Task AddRule_ValidRule_ReturnsSameHandlerInstanceForChaining()
	{
		// Arrange
		IInputProvider inputProvider = IInputProvider.Imposter().Instance();
		IOutputProvider outputProvider = IOutputProvider.Imposter().Instance();
		IValidatorRule<int> rule = IValidatorRule<int>.Imposter().Instance();
		HandlerBase<int> sut = HandlerBase<int>
			.Imposter(inputProvider, outputProvider, DefaultPrompt)
			.Instance();

		// Act
		IHandler<int> returned = sut.AddRule(rule);

		// Assert
		await Assert.That(returned).IsSameReferenceAs(sut);
	}

	[Test]
	public async Task GetValidInputAsync_ValidInputWithoutRules_DisplaysPromptAndReturnsParsedValue()
	{
		// Arrange
		IInputProviderImposter inputImposter = IInputProvider.Imposter();
		IOutputProviderImposter outputImposter = IOutputProvider.Imposter();
		HandlerBaseImposter<int> handlerImposter = HandlerBase<int>.Imposter(
			inputImposter.Instance(),
			outputImposter.Instance(),
			DefaultPrompt
		);

		// Act
		_ = inputImposter.GetInputAsync(Arg.Any).ReturnsAsync("42");
		_ = handlerImposter
			.TryParse("42", OutArg<int>.Any(), OutArg<string?>.Any())
			.Returns(
				(_, out value, out errorMessage) =>
				{
					value = 42;
					errorMessage = null;
					return true;
				}
			);

		int result = await handlerImposter.Instance().GetValidInputAsync().ConfigureAwait(false);

		// Assert
		await Assert.That(result).IsEqualTo(42);
		outputImposter.DisplayPromptAsync(DefaultPrompt, Arg.Any).Called(Count.Once());
		inputImposter.GetInputAsync(Arg.Any).Called(Count.Once());
		outputImposter.DisplayErrorAsync(Arg.Any, Arg.Any).Called(Count.Never());
	}

	[Test]
	public async Task GetValidInputAsync_ParseFailsThenSucceeds_DisplaysParseErrorAndReprompts()
	{
		// Arrange
		const string parseErrorMessage = "Please enter a valid Int32.";
		IInputProviderImposter inputImposter = IInputProvider.Imposter();
		IOutputProviderImposter outputImposter = IOutputProvider.Imposter();
		HandlerBaseImposter<int> handlerImposter = HandlerBase<int>.Imposter(
			inputImposter.Instance(),
			outputImposter.Instance(),
			DefaultPrompt
		);

		// Act
		_ = inputImposter.GetInputAsync(Arg.Any).ReturnsAsync("abc").Then().ReturnsAsync("42");
		_ = handlerImposter
			.TryParse("abc", OutArg<int>.Any(), OutArg<string?>.Any())
			.Returns(
				(_, out value, out errorMessage) =>
				{
					value = default;
					errorMessage = parseErrorMessage;
					return false;
				}
			);
		_ = handlerImposter
			.TryParse("42", OutArg<int>.Any(), OutArg<string?>.Any())
			.Returns(
				(_, out value, out errorMessage) =>
				{
					value = 42;
					errorMessage = null;
					return true;
				}
			);

		int result = await handlerImposter.Instance().GetValidInputAsync().ConfigureAwait(false);

		// Assert
		await Assert.That(result).IsEqualTo(42);
		outputImposter.DisplayPromptAsync(DefaultPrompt, Arg.Any).Called(Count.Twice());
		inputImposter.GetInputAsync(Arg.Any).Called(Count.Twice());
		outputImposter.DisplayErrorAsync(parseErrorMessage, Arg.Any).Called(Count.Once());
	}

	[Test]
	public async Task GetValidInputAsync_MultipleRules_EvaluatesInOrderAndShortCircuitsOnFirstFailure()
	{
		// Arrange
		const string firstRuleError = "Value must be positive";
		const string secondRuleError = "Value must be even";

		IInputProviderImposter inputImposter = IInputProvider.Imposter();
		IOutputProviderImposter outputImposter = IOutputProvider.Imposter();
		IValidatorRuleImposter<int> firstRuleImposter = IValidatorRule<int>.Imposter();
		IValidatorRuleImposter<int> secondRuleImposter = IValidatorRule<int>.Imposter();
		HandlerBaseImposter<int> handlerImposter = HandlerBase<int>.Imposter(
			inputImposter.Instance(),
			outputImposter.Instance(),
			DefaultPrompt
		);

		// Act
		_ = inputImposter
			.GetInputAsync(Arg.Any)
			.ReturnsAsync("-5")
			.Then()
			.ReturnsAsync("3")
			.Then()
			.ReturnsAsync("4");

		_ = handlerImposter
			.TryParse(Arg.Any, OutArg<int>.Any(), OutArg<string?>.Any())
			.Returns(
				(input, out value, out errorMessage) =>
				{
					value = int.Parse(input!, System.Globalization.CultureInfo.InvariantCulture);
					errorMessage = null;
					return true;
				}
			);

		_ = firstRuleImposter.ErrorMessage.Getter().Returns(firstRuleError);
		_ = firstRuleImposter.IsValid(Arg<int>.Is(x => x > 0)).Returns(true);
		_ = firstRuleImposter.IsValid(Arg<int>.Is(x => x <= 0)).Returns(false);

		_ = secondRuleImposter.ErrorMessage.Getter().Returns(secondRuleError);
		_ = secondRuleImposter.IsValid(Arg<int>.Is(x => x % 2 == 0)).Returns(true);
		_ = secondRuleImposter.IsValid(Arg<int>.Is(x => x % 2 != 0)).Returns(false);

		HandlerBase<int> sut = handlerImposter.Instance();
		_ = sut.AddRule(firstRuleImposter.Instance()).AddRule(secondRuleImposter.Instance());

		int result = await sut.GetValidInputAsync().ConfigureAwait(false);

		// Assert
		await Assert.That(result).IsEqualTo(4);
		outputImposter.DisplayPromptAsync(DefaultPrompt, Arg.Any).Called(Count.Exactly(3));
		outputImposter.DisplayErrorAsync(firstRuleError, Arg.Any).Called(Count.Once());
		outputImposter.DisplayErrorAsync(secondRuleError, Arg.Any).Called(Count.Once());
		firstRuleImposter.IsValid(Arg.Any).Called(Count.Exactly(3));
		secondRuleImposter.IsValid(-5).Called(Count.Never());
		secondRuleImposter.IsValid(3).Called(Count.Once());
		secondRuleImposter.IsValid(4).Called(Count.Once());
	}

	[Test]
	public async Task GetValidInputAsync_CancelledTokenBeforeStart_ThrowsOperationCanceledExceptionWithoutPrompting()
	{
		// Arrange
		IInputProviderImposter inputImposter = IInputProvider.Imposter();
		IOutputProviderImposter outputImposter = IOutputProvider.Imposter();
		HandlerBaseImposter<int> handlerImposter = HandlerBase<int>.Imposter(
			inputImposter.Instance(),
			outputImposter.Instance(),
			DefaultPrompt
		);

		// Act
		using CancellationTokenSource cts = new();
		await cts.CancelAsync().ConfigureAwait(false);

		// Assert
		_ = await Assert
			.That(() => handlerImposter.Instance().GetValidInputAsync(cts.Token))
			.Throws<OperationCanceledException>();

		outputImposter.DisplayPromptAsync(Arg.Any, Arg.Any).Called(Count.Never());
		inputImposter.GetInputAsync(Arg.Any).Called(Count.Never());
	}

	[Test]
	public async Task GetValidInput_ValidInput_ExecutesSynchronouslyAndReturnsParsedValue()
	{
		// Arrange
		IInputProviderImposter inputImposter = IInputProvider.Imposter();
		IOutputProviderImposter outputImposter = IOutputProvider.Imposter();
		HandlerBaseImposter<int> handlerImposter = HandlerBase<int>.Imposter(
			inputImposter.Instance(),
			outputImposter.Instance(),
			DefaultPrompt
		);

		// Act
		_ = inputImposter.GetInputAsync(Arg.Any).ReturnsAsync("99");
		_ = handlerImposter
			.TryParse("99", OutArg<int>.Any(), OutArg<string?>.Any())
			.Returns(
				(_, out value, out errorMessage) =>
				{
					value = 99;
					errorMessage = null;
					return true;
				}
			);

		int result = InvokeSynchronously(handlerImposter.Instance());

		// Assert
		await Assert.That(result).IsEqualTo(99);
		outputImposter.DisplayPromptAsync(DefaultPrompt, Arg.Any).Called(Count.Once());
		inputImposter.GetInputAsync(Arg.Any).Called(Count.Once());

		static int InvokeSynchronously(HandlerBase<int> handler) => handler.GetValidInput();
	}
}
